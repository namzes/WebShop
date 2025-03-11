using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebShop.API.Properties;
using Webshop.Shared.Models;


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
			app.MapGet("/Account/me", async (HttpContext context, WebShopDbContext dbContext) =>
			{
				try
				{
					var userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

					
					if (string.IsNullOrEmpty(userId))
					{
						return Results.Unauthorized();
					}

					var user = await dbContext.Users.FindAsync(userId);

					if (user == null)
					{
						return Results.NotFound();
					}

					// Create a UserDTO to return
					var userDto = new UserGetDTO()
					{
						Id = user.Id,
						Email = user.Email ?? "default@email.com",
						
					};

					
					return Results.Ok(userDto);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error: {ex}");
					return Results.Problem("An error occurred while processing your request.");
				}
			}).RequireAuthorization();
		}
	}
}
	
	
