using FilmsDomain.Model;

namespace FilmsInfrastructure.Services
{
    public class FilmDataPortServiceFactory
       : IDataPortServiceFactory<Film>

    {
        private readonly DbfilmsContext _context;
        public FilmDataPortServiceFactory(DbfilmsContext context)
        {
            _context = context;
        }
        public IImportService<Film> GetImportService(string contentType)
        {
            if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new FilmsImportService(_context);
            }
            throw new NotImplementedException($"Не розроблений сервіс імпорту екскурсій з типом контнету {contentType}");
        }
        public IExportService<Film> GetExportService(string contentType)
        {
            if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new FilmsExportService(_context);
            }
            throw new NotImplementedException($"Не розроблений сервіс імпорту екскурсій з типом контнету {contentType}");
        }

    }
}