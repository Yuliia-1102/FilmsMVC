using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilmsDomain.Model;
using FilmsInfrastructure;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System.IO;
using Microsoft.IdentityModel.Tokens;

namespace FilmsInfrastructure.Controllers
{
    public class FilmsController : Controller
    {
        private readonly DbfilmsContext _context;

        public FilmsController(DbfilmsContext context)
        {
            _context = context;
        }

        // GET: Films from Films
        public async Task<IActionResult> Index()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name");
            ViewData["CountriesFilms"] = new SelectList(_context.Countries, "Id", "Name");
            ViewData["ActorsFilms"] = new SelectList(_context.Actors, "Id", "Name");

            var dbfilmsContext = _context.Films.Include(f => f.Director).Include(f => f.Genre);
            return View(await dbfilmsContext.ToListAsync());
        }

        // GET: Films from Directors
        public async Task<IActionResult> IndexDirector(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Directors", "Index");

            ViewBag.DirectorId = id;
            ViewBag.DirectorName = name;

            var filmByDirector = _context.Films.Where(f => f.DirectorId == id).Include(f => f.Director)
                .Include(f => f.ActorsFilms).ThenInclude(f => f.Actor)
                .Include(f => f.CountriesFilms).ThenInclude(f => f.Country);

            return View(await filmByDirector.ToListAsync());
        }

        // GET: Films from Genres
        public async Task<IActionResult> IndexGenre(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Genres", "Index");

            ViewBag.GenreId = id;
            ViewBag.GenreName = name;

            var filmByGenre = _context.Films.Where(f => f.GenreId == id).Include(f => f.Genre)
                .Include(f => f.ActorsFilms).ThenInclude(f => f.Actor)
                .Include(f => f.CountriesFilms).ThenInclude(f => f.Country);

            return View(await filmByGenre.ToListAsync());
        }

        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .Include(f => f.Director)
                .Include(f => f.Genre)
                .Include(f => f.ActorsFilms)
                .ThenInclude(f => f.Actor)
                .Include(f => f.CountriesFilms)
                .ThenInclude(f => f.Country)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (film == null)
            {
                return NotFound();
            }

            var isPurchased = await IsFilmPurchased(film.Id);
            ViewData["IsPurchased"] = isPurchased;

            return View(film);
        }

        // GET: Films/Create
        public IActionResult Create(int directorId)
        {
            //ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name");
            ViewBag.DirectorId = directorId;
            ViewBag.Name = _context.Directors.Where(d => d.Id == directorId).First().Name;
           
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            ViewData["CountriesFilms"] = new SelectList(_context.Countries, "Id", "Name");
            ViewData["ActorsFilms"] = new SelectList(_context.Actors, "Id", "Name");

            return View();
        }

        // POST: Films/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int directorId, [Bind("GenreId,Name,Description,ReleaseYear,TrailerLink,Price,BoxOffice,Id")] Film film, List<int> Countries, List<int> Actors)
        {
            foreach (var countryId in Countries)
            {
                film.CountriesFilms.Add(new CountriesFilm { CountryId = countryId });
            }

            foreach (var actorsId in Actors)
            {
                film.ActorsFilms.Add(new ActorsFilm { ActorId = actorsId });
            }

            film.DirectorId = directorId;
            //if (ModelState.IsValid)
            //{
                _context.Add(film);
                await _context.SaveChangesAsync();

                return RedirectToAction("IndexDirector", "Films", new { id = directorId, name = _context.Directors.Where(d => d.Id == directorId).First().Name });
            //}
            //ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name", film.DirectorId);
            //ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", film.GenreId);
            //return View(film);
            //return RedirectToAction("IndexDirector", "Films", new { id = directorId, name = _context.Directors.Where(d => d.Id == directorId).First().Name });
        }

        public IActionResult CreateInFilm()
        {
            ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name");
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            ViewData["CountriesFilms"] = new SelectList(_context.Countries, "Id", "Name");
            ViewData["ActorsFilms"] = new SelectList(_context.Actors, "Id", "Name");

            return View();
        }

        // POST: Films/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInFilm([Bind("DirectorId,GenreId,Name,Description,ReleaseYear,TrailerLink,Price,BoxOffice,Id")] Film film, List<int> Countries, List<int> Actors)
        {
            foreach (var countryId in Countries)
            {
                film.CountriesFilms.Add(new CountriesFilm { CountryId = countryId });
            }

            foreach (var actorsId in Actors)
            {
                film.ActorsFilms.Add(new ActorsFilm { ActorId = actorsId });
            }

            //if (ModelState.IsValid)
            //{
            _context.Add(film);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Films");
            //}
            //ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name", film.DirectorId);
            //ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", film.GenreId);
            //return View(film);
            //return RedirectToAction("IndexDirector", "Films", new { id = directorId, name = _context.Directors.Where(d => d.Id == directorId).First().Name });
        }

        // GET: Films/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name", film.DirectorId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", film.GenreId);
			ViewData["CountriesFilms"] = new SelectList(_context.Countries, "Id", "Name", film.CountriesFilms);
			ViewData["ActorsFilms"] = new SelectList(_context.Actors, "Id", "Name", film.ActorsFilms);
			return View(film);
        }

        // POST: Films/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GenreId,DirectorId,Name,Description,ReleaseYear,TrailerLink,Price,BoxOffice,Id")] Film film, List<int> Countries, List<int> Actors)
        {
            if (id != film.Id)
            {
                return NotFound();
            }

           var filmToUpdate = await _context.Films
                               .Include(f => f.CountriesFilms)
                               .Include(f => f.ActorsFilms)
                               .FirstOrDefaultAsync(f => f.Id == id);

            if (filmToUpdate == null)
            {
                return NotFound();
            }

            // Оновлення акторів
            var currentActorIds = filmToUpdate.ActorsFilms.Select(af => af.ActorId).ToList();
            var actorsToAdd = Actors.Except(currentActorIds).ToList();
            var actorsToRemove = currentActorIds.Except(Actors).ToList();

            foreach (var actorId in actorsToRemove)
            {
                var actorFilm = filmToUpdate.ActorsFilms.FirstOrDefault(af => af.ActorId == actorId);
                if (actorFilm != null) _context.ActorsFilms.Remove(actorFilm);
            }

            foreach (var actorId in actorsToAdd)
            {
                filmToUpdate.ActorsFilms.Add(new ActorsFilm { FilmId = id, ActorId = actorId });
            }

            // Оновлення країн
            var currentCountryIds = filmToUpdate.CountriesFilms.Select(cf => cf.CountryId).ToList();
            var countriesToAdd = Countries.Except(currentCountryIds).ToList();
            var countriesToRemove = currentCountryIds.Except(Countries).ToList();

            foreach (var countryId in countriesToRemove)
            {
                var countryFilm = filmToUpdate.CountriesFilms.FirstOrDefault(cf => cf.CountryId == countryId);
                if (countryFilm != null) _context.CountriesFilms.Remove(countryFilm);
            }

            foreach (var countryId in countriesToAdd)
            {
                filmToUpdate.CountriesFilms.Add(new CountriesFilm { FilmId = id, CountryId = countryId });
            }

            // Зберегти зміни
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        /*
        public async Task<IActionResult> Edit(int id, [Bind("GenreId,DirectorId,Name,Description,ReleaseYear,TrailerLink,Price,BoxOffice,Id")] Film film, List<int> Countries, List<int> Actors)
        {
            if (id != film.Id)
            {
                return NotFound();
            }

            var filmToUpdate = await _context.Films
                               .Include(f => f.CountriesFilms)
                               .Include(f => f.ActorsFilms)
                               .FirstOrDefaultAsync(f => f.Id == id);
            
            _context.Update(filmToUpdate);
            await _context.SaveChangesAsync();
            if (ModelState.IsValid)
            {
            try
                {
                    _context.Update(film);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.Id))
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
            ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name", film.DirectorId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", film.GenreId);
			ViewData["CountriesFilms"] = new SelectList(_context.Countries, "Id", "Name", film.CountriesFilms);
			ViewData["ActorsFilms"] = new SelectList(_context.Actors, "Id", "Name", film.ActorsFilms);
			return View(film);
        }*/

        // GET: Films/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .Include(f => f.Director)
                .Include(f => f.Genre)
                .Include(f => f.ActorsFilms)
                .ThenInclude(f => f.Actor)
                .Include(f => f.CountriesFilms)
                .ThenInclude(f => f.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

           

            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            /*var film = await _context.Films.FindAsync(id);
            if (film != null)
            {
                _context.Films.Remove(film);
            }*/

            var countriesFilms = await _context.CountriesFilms
                                .Where(cf => cf.FilmId == id)
                                .ToListAsync();

            var actorsFilms = await _context.ActorsFilms
                                .Where(cf => cf.FilmId == id)
                                .ToListAsync();

            _context.CountriesFilms.RemoveRange(countriesFilms);
            _context.ActorsFilms.RemoveRange(actorsFilms);

            await _context.SaveChangesAsync();

            var film = await _context.Films.FindAsync(id);
            _context.Films.Remove(film);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return _context.Films.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> Search(int? genre, int? director, int? country, int? actor)
        {

            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name");
            ViewData["CountriesFilms"] = new SelectList(_context.Countries, "Id", "Name");
            ViewData["ActorsFilms"] = new SelectList(_context.Actors, "Id", "Name");

            var films = _context.Films.AsQueryable();

            if (genre.HasValue)
                films = films.Where(f => f.GenreId == genre);
            if (director.HasValue)
                films = films.Where(f => f.DirectorId == director);
            if (country.HasValue)
                films = films.Where(f => f.CountriesFilms.Any(cf => cf.CountryId == country));
            if (actor.HasValue)
                films = films.Where(f => f.ActorsFilms.Any(af => af.ActorId == actor));

            return View("Index", films);
        }

        public async Task<bool> IsFilmPurchased(int filmId)
        {
            int customerId = 25;
            return await _context.Preorders.AnyAsync(p => p.FilmId == filmId && p.CustomerId == customerId && p.Status == "Куплено");
        }

    }
}
