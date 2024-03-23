using FilmsDomain.Model;

namespace FilmsInfrastructure.Services
{
    public interface IExportService<TEntity>
    where TEntity : Entity
    {
        Task WriteToAsync(Stream stream, CancellationToken cancellationToken);
    }
}

