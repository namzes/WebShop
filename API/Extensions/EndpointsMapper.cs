using Microsoft.EntityFrameworkCore;
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
				//TODO: Make ToDto Extension method
				var products = await dbContext.Products.ToListAsync();
				return Results.Ok(products);
			});
			app.MapGet("/api/Products/{id}", async (int id,WebShopDbContext dbContext) =>
			{
				//TODO: Make ToDto Extension method
				var product = await dbContext.Products.FindAsync(id);
				return Results.Ok(product);
			});
		}
	}
}
	
	
