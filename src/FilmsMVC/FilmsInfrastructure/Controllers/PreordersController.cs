using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilmsDomain.Model;
using FilmsInfrastructure;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FilmsInfrastructure.Controllers
{
    public class PreordersController : Controller
    {
        private readonly DbfilmsContext _context;

        public PreordersController(DbfilmsContext context)
        {
            _context = context;
        }

        // GET: Preorders
        public async Task<IActionResult> Index()
        {
            //int customerId = 32;

            var dbfilmsContext = _context.Preorders
                                         .Include(p => p.Customer)
                                         .Include(p => p.Film);

            return View(await dbfilmsContext.ToListAsync());
        }


        // GET: Preorders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preorder = await _context.Preorders
                .Include(p => p.Customer)
                .Include(p => p.Film)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (preorder == null)
            {
                return NotFound();
            }

            return View(preorder);
        }

		[HttpPost]
        public async Task<IActionResult> AddToOrder(int filmId)
        {
            var film = await _context.Films.FindAsync(filmId);
            if (film == null)
            {
                return NotFound();
            }

            var customer = _context.Customers.Include(c => c.Preorders).FirstOrDefault(c => c.Email == User.Identity.Name);

            var existingPreorder = await _context.Preorders
            .FirstOrDefaultAsync(p => p.FilmId == filmId && p.CustomerId == customer.Id);

            if (existingPreorder == null)
            {
                var preorder = new Preorder
                {
                    FilmId = filmId,
                    CustomerId = customer.Id, 
                    Status = null,
                    PreorderDate = DateOnly.FromDateTime(DateTime.Now)
                };
                _context.Preorders.Add(preorder);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Preorders");
        }


        [HttpPost]
        public async Task<IActionResult> Order(List<int> preordersId)
        {
            var preorders = await _context.Preorders
                .Where(p => preordersId.Contains(p.Id))
                .Include(p => p.Film)
                .ToListAsync();

            if (!preorders.Any()) 
            {
                return RedirectToAction("Index", "Films");
            }

            if (preorders.All(p => p.Status == "Куплено"))
            {
                return RedirectToAction("Index", new { message = "Всі фільми вже куплені." });
            }

            var totalPrice = preorders.Where(p => p.Status == null).Sum(p => p.Film.Price);

            if (totalPrice > 0)
            {
                TempData["TotalPrice"] = totalPrice.ToString();
                TempData["PreordersId"] = String.Join(",", preorders.Where(p => p.Status == null).Select(p => p.Id));
                return RedirectToAction("PaymentInfo");
            }

            return RedirectToAction("Index", new { message = "Немає фільмів до покупки." });
        }

        public IActionResult PaymentInfo()
        {
            return View();
        }

        public async Task<IActionResult> ProcessPayment(string CardNumber)
        {
            var preordersIdStr = TempData["PreordersId"] as string;
            var preordersIdList = preordersIdStr?.Split(',').Select(int.Parse).ToList();

            if (preordersIdList != null && preordersIdList.Any())
            {
                var preordersToDelete = _context.Preorders.Where(p => preordersIdList.Contains(p.Id)).ToList();
               
                foreach (var preorder in preordersToDelete)
                {
                    preorder.Status = "Куплено";
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Preorders", new { message = "Оплата пройшла успішно. Ваші фільми доступні до перегляду." });
        }

        // GET: Preorders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email");
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Name");
            ViewData["Status"] = new SelectList(new List<string> { "", "Куплено" });
            return View();
        }

        // POST: Preorders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FilmId,CustomerId,Id,Status")] Preorder preorder)
        {
            var existingPreorder = _context.Preorders
        .Include(p => p.Customer)
        .Include(p => p.Film)
        .FirstOrDefault(p => p.CustomerId == preorder.CustomerId && p.FilmId == preorder.FilmId);

            if (existingPreorder != null)
            {
                ModelState.AddModelError(string.Empty, "Замовлення з такою електронною поштою клієнта та назвою фільму вже існує.");
            }

            if (ModelState.IsValid)
            {
                preorder.PreorderDate = DateOnly.FromDateTime(DateTime.Now);
                _context.Add(preorder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email", preorder.CustomerId);
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Name", preorder.FilmId);
            ViewData["Status"] = new SelectList(new List<string> { "", "Куплено" }, preorder.Status);
            return View(preorder);
        }

        // GET: Preorders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preorder = await _context.Preorders.FindAsync(id);
            if (preorder == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email", preorder.CustomerId);
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Name", preorder.FilmId);
            ViewData["Status"] = new SelectList(new List<string> { "", "Куплено" }, preorder.Status);
            return View(preorder);
        }

        // POST: Preorders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FilmId,CustomerId,PreorderDate,Id,Status")] Preorder preorder)
        {
            if (id != preorder.Id)
            {
                return NotFound();
            }

            var existingPreorder = _context.Preorders
        .Include(p => p.Customer)
        .Include(p => p.Film)
        .FirstOrDefault(p => p.CustomerId == preorder.CustomerId && p.FilmId == preorder.FilmId && p.Id != preorder.Id);

            if (existingPreorder != null)
            {
                ModelState.AddModelError(string.Empty, "Замовлення з такою електронною поштою клієнта та назвою фільму вже існує.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(preorder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PreorderExists(preorder.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email", preorder.CustomerId);
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Name", preorder.FilmId);
            ViewData["Status"] = new SelectList(new List<string> { "", "Куплено" }, preorder.Status);
            return View(preorder);
        }

        // GET: Preorders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preorder = await _context.Preorders
                .Include(p => p.Customer)
                .Include(p => p.Film)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (preorder == null)
            {
                return NotFound();
            }

            return View(preorder);
        }

        // POST: Preorders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var preorder = await _context.Preorders.FindAsync(id);
            if (preorder != null)
            {
                _context.Preorders.Remove(preorder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PreorderExists(int id)
        {
            return _context.Preorders.Any(e => e.Id == id);
        }
    }
}
