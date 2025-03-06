using Microsoft.EntityFrameworkCore;
using WebShop.API.Properties;


namespace WebShop.API.Extensions
{
	public static class EndpointsMapper
	{
		public static void MapEndpoints(this WebApplication app)
		{
			app.MapGet("/api/Products", async (WebShopDbContext dbContext) =>
			{
				
				var products = await dbContext.Products.ToListAsync();
				return Results.Ok(products.ToProductDTOs());
			});
			app.MapGet("/api/Products/{id}", async (int id,WebShopDbContext dbContext) =>
			{
				
				var product = await dbContext.Products.FindAsync(id);
				if (product == null)
				{
					return Results.NotFound();
				}
				return Results.Ok(product.ToProductDTO());
			});
		}
	}
}
	
	
