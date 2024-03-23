using ClosedXML.Excel;
using FilmsDomain.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FilmsInfrastructure.Services
{
    public class FilmsExportService : IExportService<Film>
    {
        private readonly DbfilmsContext _context;

        public FilmsExportService(DbfilmsContext context)
        {
            _context = context;
        }

        private static readonly IReadOnlyList<string> HeaderNames = new string[]
        {
            "Назва фільму",
            "Рік виходу",
            "Жанр",
            "Режисер",
            "Опис",
            "Ціна",
            "Трейлер",
            "Касовий збір"
            //"Актори",
            //"Країни виробництва"
        };

        private static void WriteHeader(IXLWorksheet worksheet)
        {
            for (int columnIndex = 0; columnIndex < HeaderNames.Count; columnIndex++)
            {
                worksheet.Cell(1, columnIndex + 1).Value = HeaderNames[columnIndex];
                worksheet.Column(columnIndex + 1).Width = 25;
            }
            worksheet.Row(1).Style.Font.Bold = true;
        }

        private void WriteFilm(IXLWorksheet worksheet, Film film, int rowIndex)
        {
            var columnIndex = 1;
			worksheet.Cell(rowIndex, columnIndex++).Value = film.Name;
            worksheet.Cell(rowIndex, columnIndex++).Value = film.ReleaseYear;
            worksheet.Cell(rowIndex, columnIndex++).Value = film.Genre.Name;
            worksheet.Cell(rowIndex, columnIndex++).Value = film.Director.Name;
            worksheet.Cell(rowIndex, columnIndex++).Value = film.Description;
            worksheet.Cell(rowIndex, columnIndex++).Value = film.Price;
            worksheet.Cell(rowIndex, columnIndex++).Value = film.TrailerLink;
            worksheet.Cell(rowIndex, columnIndex++).Value = film.BoxOffice;
			//worksheet.Cell(rowIndex, columnIndex++).Value = film.Id;
			//worksheet.Cell(rowIndex, columnIndex++).Value = string.Join(", ", film.ActorsFilms.Select(af => af.Actor.Name));
			//worksheet.Cell(rowIndex, columnIndex).Value = string.Join(", ", film.CountriesFilms.Select(cf => cf.Country.Name));
		}

        private void WriteFilms(IXLWorksheet worksheet, ICollection<Film> films)
        {
            WriteHeader(worksheet);
            int rowIndex = 2;
            foreach (var film in films)
            {
                WriteFilm(worksheet, film, rowIndex++);
            }
        }

        public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException("Cannot write to the file.");
            }

            var films = await _context.Films
                .Include(f => f.Genre)
                .Include(f => f.Director)
                //.Include(f => f.ActorsFilms).ThenInclude(af => af.Actor)
                //.Include(f => f.CountriesFilms).ThenInclude(cf => cf.Country)
                .ToListAsync(cancellationToken);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Фільми");
            WriteFilms(worksheet, films);

            workbook.SaveAs(stream);
        }
    }
}
