using Microsoft.EntityFrameworkCore;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Concrete
{
	public class EFDbContext : DbContext
	{
		public EFDbContext(DbContextOptions<EFDbContext> options) : base(options)
		{
		}

		public DbSet<Product> Products { get; set; }
	}
}
