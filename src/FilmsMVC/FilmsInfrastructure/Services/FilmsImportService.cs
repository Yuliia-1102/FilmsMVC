using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Vml.Office;
using DocumentFormat.OpenXml.Wordprocessing;
using FilmsDomain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FilmsInfrastructure.Services
{
    public class FilmsImportService : IImportService<FilmsDomain.Model.Film>
    {
        private readonly DbfilmsContext _context;
        private static bool _isValid;
        private static string? _validationErrorStr;

        public FilmsImportService(DbfilmsContext context)
        {
            _validationErrorStr = "";
            _isValid = true;
            _context = context;
        }

        public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanRead)
            {
                throw new ArgumentException("Дані не можуть бути прочитані", nameof(stream));
            }

            using (XLWorkbook workBook = new XLWorkbook(stream))
            {
                foreach (IXLWorksheet worksheet in workBook.Worksheets)
                {
                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        AddFilm(row, cancellationToken);
                    }
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

		/*private bool IsFilm(Film e)
        {
            var film = _context.Films.FirstOrDefault(film => film.Name == e.Name);

            if (film == null)
            {
                return false;
            }

            return true;
        }*/

		private void AddFilm(IXLRow row, CancellationToken cancellationToken)
		{
			Film existingFilm = _context.Films.FirstOrDefault(film => film.Name == GetFilmName(row));

			if (existingFilm != null)
			{
				existingFilm.Description = GetFilmDescr(row);
				existingFilm.GenreId = GetGenreId(row);
				existingFilm.DirectorId = GetDirectorId(row);
				existingFilm.ReleaseYear = GetReleaseYear(row);
				existingFilm.Price = GetFilmPrice(row);
				existingFilm.TrailerLink = GetTrailerLink(row);
				existingFilm.BoxOffice = GetBoxOffice(row);
			}
			else if (_isValid)
			{
				Film newFilm = new Film
				{
					Name = GetFilmName(row),
					Description = GetFilmDescr(row),
					GenreId = GetGenreId(row),
					DirectorId = GetDirectorId(row),
					ReleaseYear = GetReleaseYear(row),
					Price = GetFilmPrice(row),
					TrailerLink = GetTrailerLink(row),
					BoxOffice = GetBoxOffice(row)
				};

				_context.Films.Add(newFilm);
			}

			if (!_isValid)
			{
				throw new ArgumentException(_validationErrorStr);
			}
		}

		/*private void UpdateExistingFilm(Film existingFilm, IXLRow row)
		{
			existingFilm.Name = GetFilmName(row); 
			existingFilm.Description = GetFilmDescr(row);
			existingFilm.GenreId = GetGenreId(row);
			existingFilm.DirectorId = GetDirectorId(row);
			existingFilm.ReleaseYear = GetReleaseYear(row);
			existingFilm.Price = GetFilmPrice(row);
			existingFilm.TrailerLink = GetTrailerLink(row);
			existingFilm.BoxOffice = GetBoxOffice(row);
		}

		private void AddNewFilm(IXLRow row)
		{
			Film newFilm = new Film
			{
				Name = GetFilmName(row),
				Description = GetFilmDescr(row),
				GenreId = GetGenreId(row),
				DirectorId = GetDirectorId(row),
				ReleaseYear = GetReleaseYear(row),
				Price = GetFilmPrice(row),
				TrailerLink = GetTrailerLink(row),
				BoxOffice = GetBoxOffice(row)
			};

			_context.Films.Add(newFilm);
		}

        private int GetFilmId(IXLRow row)
		{
			return int.Parse(row.Cell(1).Value.ToString().Trim());
		}*/

		private static string GetFilmName(IXLRow row)
		{
			int rowNumber = row.RowNumber();
			string value = row.Cell(1).Value.ToString();

			if (string.IsNullOrEmpty(value))
			{
				_isValid = false;
				_validationErrorStr += $"Назва фільму не може бути пуста, рядок: {rowNumber}.\n";
				return "";
			}

			return value;
		}


		private short GetReleaseYear(IXLRow row)
        {
            int rowNumber = row.RowNumber();
            short releaseYear;
            if (short.TryParse(row.Cell(2).Value.ToString(), out releaseYear) &&
                releaseYear >= 1900 && releaseYear <= DateTime.Now.Year)
            {
                return releaseYear;
            }
            else 
            {
                _isValid = false;
                _validationErrorStr += $"Некоректний рік виходу фільму в {rowNumber} рядку.\n";
                return 0;
            }
        }

        private int GetGenreId(IXLRow row)
        {
			int rowNumber = row.RowNumber();
			var genreName = row.Cell(3).Value.ToString().Trim();

            if (!string.IsNullOrWhiteSpace(genreName))
            {
                var genre = _context.Genres.FirstOrDefault(g => g.Name.Equals(genreName));

                if (genre != null)
                {
                    return genre.Id;
                }
                else
                {
                    _isValid = false;
                    _validationErrorStr += $"Жанру '{genreName}' не знайдено, рядок: {rowNumber}.\n";
                    return -1;
                }
            }
            else
            {
                _isValid = false;
                _validationErrorStr += $"Назва жанру не може бути порожньою, рядок: {rowNumber}.\n";
                return -1;
            }
        }

        private int GetDirectorId(IXLRow row)
        {
			int rowNumber = row.RowNumber();
			var directorName = row.Cell(4).Value.ToString().Trim();

            if (!string.IsNullOrWhiteSpace(directorName))
            {
                var director = _context.Directors.FirstOrDefault(d => d.Name.Equals(directorName));

                if (director != null)
                {
                    return director.Id;
                }
                else
                {
                    _isValid = false;
                    _validationErrorStr += $"Режисера з іменем '{directorName}' не знайдено, рядок: {rowNumber}.\n";
                    return -1;
                }
            }
            else
            {
                _isValid = false;
                _validationErrorStr += $"Ім'я режисера не може бути порожнім, рядок: {rowNumber}.\n";
                return -1;
            }
        }

		private static string GetFilmDescr(IXLRow row)
		{
			int rowNumber = row.RowNumber();
			string description = row.Cell(5).Value.ToString();

			if (string.IsNullOrWhiteSpace(description))
			{
				_isValid = false;
				_validationErrorStr += $"Опис фільму не може бути пустим, рядок: {rowNumber}.\n";
				return "";
			}

			return description;
		}

		private static float GetFilmPrice(IXLRow row)
		{
			int rowNumber = row.RowNumber();
			string valueStr = row.Cell(6).Value.ToString().Trim(); 

			if (string.IsNullOrEmpty(valueStr))
			{
				_isValid = false;
				_validationErrorStr += $"Ціна не може бути порожньою, рядок: {rowNumber}.\n";
				return 0;
			}

			if (float.TryParse(valueStr, NumberStyles.Any, CultureInfo.InvariantCulture, out float value))
			{
				if (value <= 0)
				{
					_isValid = false;
					_validationErrorStr += $"Ціна фільму не може бути від'ємною чи нулем, рядок: {rowNumber}.\n";
					return 0;
				}

				int decimalIndex = valueStr.IndexOf('.');
				if (decimalIndex != -1 && valueStr.Length - decimalIndex - 1 > 2)
				{
					_isValid = false;
					_validationErrorStr += $"Ціна фільму має бути з максимум двома десятковими знаками, рядок: {rowNumber}.\n";
					return 0;
				}

				return value;
			}
			else
			{
				_isValid = false;
				_validationErrorStr += $"Ціна фільму має неправильний формат, рядок: {rowNumber}.\n";
				return 0;
			}
		}


		private string GetTrailerLink(IXLRow row)
        {
			int rowNumber = row.RowNumber();
			var trailerLink = row.Cell(7).Value.ToString();
            if (string.IsNullOrEmpty(trailerLink) || trailerLink.StartsWith("https://"))
            {
                return trailerLink;
            }
            else
            {
                _isValid = false;
                _validationErrorStr += $"Некоректне посилання на трейлер, рядок: {rowNumber}.\n";
                return string.Empty;
            }
        }

		private int? GetBoxOffice(IXLRow row)
		{
			int rowNumber = row.RowNumber();
			var boxOfficeValue = row.Cell(8).Value.ToString();
			if (string.IsNullOrWhiteSpace(boxOfficeValue))
			{
				return null;
			}
			else if (int.TryParse(boxOfficeValue, out int boxOffice) && boxOffice > 0)
			{
				return boxOffice;
			}
			else
			{
				_isValid = false;
				_validationErrorStr += $"Некоректний касовий збір, рядок: {rowNumber}.\n";
				return null;
			}
		}


	}
}