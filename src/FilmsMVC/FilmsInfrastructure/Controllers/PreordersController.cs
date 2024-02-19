using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilmsDomain.Model;
using FilmsInfrastructure;

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
            var dbfilmsContext = _context.Preorders.Include(p => p.Customer).Include(p => p.Film);
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

            // Спроба знайти останнього покупця у базі даних
            var lastCustomer = await _context.Customers.OrderByDescending(c => c.Id).FirstOrDefaultAsync();

            int customerId;
            if (lastCustomer != null)
            {
                // Якщо існують покупці, створюємо нового з ID на 1 більше
                customerId = lastCustomer.Id + 1;
            }
            else
            {
                // Якщо покупців немає, створюємо першого з ID 1
                customerId = 1;
            }

            // Перевіряємо, чи вже існує покупець з таким ID (теоретично не повинно бути, але перевірка не зашкодить)
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
            {
                // Якщо покупець з таким ID не існує, створюємо нового
                customer = new Customer { /*Id = customerId, інші поля*/ };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            var preorder = new Preorder
            {
                FilmId = filmId,
                CustomerId = customerId,
                PreorderDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Preorders.Add(preorder);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Preorders");
        }


        // GET: Preorders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email");
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Description");
            return View();
        }

        // POST: Preorders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FilmId,CustomerId,PreorderDate,Id")] Preorder preorder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(preorder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email", preorder.CustomerId);
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Description", preorder.FilmId);
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
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Description", preorder.FilmId);
            return View(preorder);
        }

        // POST: Preorders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FilmId,CustomerId,PreorderDate,Id")] Preorder preorder)
        {
            if (id != preorder.Id)
            {
                return NotFound();
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
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Description", preorder.FilmId);
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
