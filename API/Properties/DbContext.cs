using Microsoft.EntityFrameworkCore;
using Webshop.Shared.Models;

namespace WebShop.API.Properties
{
	public class WebShopDbContext : DbContext
	{
		private DbSet<Product> Products { get; set; } = null!;
		private DbSet<User> Users { get; set; } = null!;
		private DbSet<Cart> Cart { get; set; } = null!;
		private DbSet<CartProduct> CartProducts { get; set; } = null!;



	}
}
