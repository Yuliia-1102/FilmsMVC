using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmsInfrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly DbfilmsContext _context;

        public ChartController(DbfilmsContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var directors = _context.Directors.Include(d => d.Films).ToList();
            List<object> directFilms = new List<object>();

            directFilms.Add(new[] { "Режисери", "Кількість фільмів" });
            foreach(var f in directors)
            {
                directFilms.Add(new object[] { f.Name, f.Films.Count() });
            }
            return new JsonResult(directFilms);
        }

        [HttpGet("FilmsByYear")]
        public IActionResult GetFilmsByYear()
        {
            var result = _context.Films
                .GroupBy(f => f.ReleaseYear)
                .Select(group => new { Year = group.Key, Count = group.Count() })
                .OrderBy(x => x.Year)
                .ToList();

            return Ok(result);
        }
    }
}
