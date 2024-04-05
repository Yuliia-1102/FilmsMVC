using FilmsDomain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FilmsInfrastructure.Models;

namespace FilmsInfrastructure
{
	public class IdentityContext : IdentityDbContext<User>
	{
		public IdentityContext(DbContextOptions<IdentityContext> options)
			: base(options)
		{
			Database.EnsureCreated();
		}
	}
}
