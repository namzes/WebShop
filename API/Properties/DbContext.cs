using Microsoft.EntityFrameworkCore;
using Webshop.Shared.Models;

namespace WebShop.API.Properties
{
	public class WebShopDbContext : DbContext
	{
		public WebShopDbContext(DbContextOptions<WebShopDbContext> options)
			: base(options)
		{
		}
		public DbSet<Product> Products { get; set; } = null!;
		public DbSet<User> Users { get; set; } = null!;
		public DbSet<Cart> Cart { get; set; } = null!;
		public DbSet<CartProduct> CartProducts { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>()
				.Property(p => p.Price)
				.HasColumnType("decimal(18,2)");
		}
	}
}
