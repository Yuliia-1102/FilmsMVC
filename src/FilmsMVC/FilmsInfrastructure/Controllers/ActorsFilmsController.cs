using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilmsDomain.Model;
using FilmsInfrastructure;
using Microsoft.AspNetCore.Authorization;

namespace FilmsInfrastructure.Controllers
{
    [Authorize(Roles = "admin, стажер(-ка)")]
    public class ActorsFilmsController : Controller
    {
        private readonly DbfilmsContext _context;

        public ActorsFilmsController(DbfilmsContext context)
        {
            _context = context;
        }

        // GET: ActorsFilms
        public async Task<IActionResult> Index()
        {
            var dbfilmsContext = _context.ActorsFilms.Include(a => a.Actor).Include(a => a.Film);
            return View(await dbfilmsContext.ToListAsync());
        }

        // GET: ActorsFilms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actorsFilm = await _context.ActorsFilms
                .Include(a => a.Actor)
                .Include(a => a.Film)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actorsFilm == null)
            {
                return NotFound();
            }

            return View(actorsFilm);
        }

        // GET: ActorsFilms/Create
        public IActionResult Create()
        {
            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name");
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Name");
            return View();
        }

        // POST: ActorsFilms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FilmId,ActorId,Role,Id")] ActorsFilm actorsFilm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actorsFilm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name", actorsFilm.ActorId);
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Name", actorsFilm.FilmId);
            return View(actorsFilm);
        }

        // GET: ActorsFilms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actorsFilm = await _context.ActorsFilms.FindAsync(id);
            if (actorsFilm == null)
            {
                return NotFound();
            }
            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name", actorsFilm.ActorId);
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Name", actorsFilm.FilmId);
            return View(actorsFilm);
        }

        // POST: ActorsFilms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FilmId,ActorId,Role,Id")] ActorsFilm actorsFilm)
        {
            if (id != actorsFilm.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(actorsFilm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorsFilmExists(actorsFilm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name", actorsFilm.ActorId);
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Name", actorsFilm.FilmId);
            return View(actorsFilm);
        }

        // GET: ActorsFilms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actorsFilm = await _context.ActorsFilms
                .Include(a => a.Actor)
                .Include(a => a.Film)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actorsFilm == null)
            {
                return NotFound();
            }

            return View(actorsFilm);
        }

        // POST: ActorsFilms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actorsFilm = await _context.ActorsFilms.FindAsync(id);
            if (actorsFilm != null)
            {
                _context.ActorsFilms.Remove(actorsFilm);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorsFilmExists(int id)
        {
            return _context.ActorsFilms.Any(e => e.Id == id);
        }
    }
}
