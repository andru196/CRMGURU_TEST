using Microsoft.EntityFrameworkCore;


namespace CRMGURU_TEST
{
	public class CountriesDbContext : DbContext
	{
		public CountriesDbContext(DbContextOptions dco) : base(dco) => Database.EnsureCreated();

		public DbSet<Country> Countries { get; set; }
		public DbSet<City> Cities { get; set; }
		public DbSet<Region> Regions { get; set; }

		public virtual void LoadAll()
		{
			Countries.Load();
			Cities.Load();
			Regions.Load();
		}
	}
}
