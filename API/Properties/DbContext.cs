
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Webshop.Shared.Models;

namespace WebShop.API.Properties
{
	public class WebShopDbContext : IdentityDbContext<User>
	{
		public WebShopDbContext(DbContextOptions<WebShopDbContext> options)
			: base(options)
		{
		}
		public DbSet<Product> Products { get; set; } = null!;
		public DbSet<CartProduct> CartProducts { get; set; } = null!;
		public DbSet<Order> Orders { get; set; } = null!;
		public DbSet<OrderedProduct> OrderedProducts { get; set; } = null!;
		public DbSet<Sale> Sales { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Product>()
				.Property(p => p.Price)
				.HasColumnType("decimal(18,2)");

			modelBuilder.Entity<Sale>()
				.Property(s => s.SaleRate)
				.HasPrecision(3, 2);
		}
	}
	
}
