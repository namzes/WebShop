using API;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Webshop.Shared.Models;
using WebShop.API.Properties;

namespace WebShop.API.Extensions
{
	public static class EndpointsMapper
	{
		public static void MapEndpoints(this WebApplication app)
		{
			app.MapGet("/", () => "Hello World!");
			app.MapGet("/api/Products", async (WebShopDbContext dbContext) =>
			{
				var products = await dbContext.Products.ToListAsync();
				return Results.Ok(products);
			});
			app.MapGet("/Products/{id}", async (int id,WebShopDbContext dbContext) =>
			{
				var product = await dbContext.Products.FindAsync(id);
				return Results.Ok(product);
			});
			app.MapGet("/User/Create", (string username, string email, string password, string address) => TypedResults.Ok(CreateUser(username, email, password, address)));
		}

		public static User CreateUser(string username, string email, string password, string address)
		{
			return new User()
			{
				Username = username,
				Email = email,
				Password = password,
				Address = address,
				Cart = new Cart()
			};
		}
	}
}
	
	
