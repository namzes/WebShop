using API;
using System.Xml.Linq;
using Webshop.Shared.Models;

namespace WebShop.API.Extensions
{
	public static class EndpointsMapper
	{
		public static void MapEndpoints(this WebApplication app)
		{
			app.MapGet("/", () => "Hello World!");
			app.MapGet("/Products", () => TypedResults.Ok(GetProducts()));
			app.MapGet("/Products/{id}", (int id) => TypedResults.Ok(GetProduct(id)));
			app.MapGet("/User/Create", (string username, string email, string password, string address) => TypedResults.Ok(CreateUser(username, email, password, address)));
		}

		public static List<Product> GetProducts()
		{
			return new List<Product>
			{
				new Product
				{
					Id = 1, Name = "Product 1", Description = "A flower", ImageUrl = "lib/images/daisy.jpg", Price = 100
				},
				new Product { Id = 2, Name = "Product 2", Description = "A tree", ImageUrl = "lib/images/daisy.jpg", Price = 200 },
				new Product { Id = 3, Name = "Product 3", Description = "A bush", ImageUrl = "lib/images/daisy.jpg", Price = 300 },
				new Product
				{
					Id = 4, Name = "Product 4", Description = "A shrub", ImageUrl = "lib/images/daisy.jpg", Price = 400
				},
				new Product
				{
					Id = 5, Name = "Product 5", Description = "A plant", ImageUrl = "lib/images/daisy.jpg", Price = 500
				},
				new Product
				{
					Id = 6, Name = "Product 6", Description = "A flower", ImageUrl = "lib/images/daisy.jpg", Price = 600
				},
				new Product { Id = 7, Name = "Product 7", Description = "A tree", ImageUrl = "lib/images/daisy.jpg", Price = 700 },
				new Product { Id = 8, Name = "Product 8", Description = "A bush", ImageUrl = "lib/images/daisy.jpg", Price = 800 },
				new Product
				{
					Id = 9, Name = "Product 9", Description = "A shrub", ImageUrl = "lib/images/daisy.jpg", Price = 900
				},
				new Product
				{
					Id = 10, Name = "Product 10", Description = "A plant", ImageUrl = "lib/images/daisy.jpg", Price = 1000
				}
			};
		}

		public static Product GetProduct(int id)
		{
			return GetProducts().FirstOrDefault(p => p.Id == id) ?? throw new ArgumentOutOfRangeException(nameof(id), $"Product with Id {id} not found.");
		}
		public static User CreateUser(string username, string email, string password, string address)
		{
			return new User(username, email, password, address)
			{
				Username = username,
				Email = email,
				Password = password,
				Address = address,
			};
		}
	}
}
	
	
