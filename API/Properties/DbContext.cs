
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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Product>()
				.Property(p => p.Price)
				.HasColumnType("decimal(18,2)");
		}
	}
	public class User : IdentityUser
	{
	}
	public class Product
	{
		public int Id { get; set; }
		[StringLength(200)]
		public required string Name { get; set; }
		[StringLength(200)]
		public required string Description { get; set; }
		public decimal Price { get; set; }
		[StringLength(200)]
		public required string ImageUrl { get; set; }
		public int Stock { get; set; }
	}

	public class CartProduct
	{
		public int Id { get; set; }
		public int Quantity { get; set; } = 1;
		public required User User { get; set; }
		public required Product Product { get; set; }
	}
	public class Order
	{
		public int Id { get; set; }
		public required User User { get; set; }
		public List<CartProduct> OrderedProducts { get; set; } = new();
	}
}
