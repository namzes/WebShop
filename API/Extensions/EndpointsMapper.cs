using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
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
			app.MapGet("/api/Products/{id}", async (int id, WebShopDbContext dbContext) =>
			{

				var product = await dbContext.Products.FindAsync(id);
				if (product == null)
				{
					return Results.NotFound();
				}
				return Results.Ok(product.ToProductDTO());
			});
			app.MapGet("/Account/me", async (ClaimsPrincipal userPrincipal, WebShopDbContext dbContext) =>
			{
				try
				{
					var userId = userPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;


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

			app.MapGet("/api/GetCart", async (HttpContext httpContext, WebShopDbContext dbContext) =>
			{
				var userPrincipal = httpContext.User;
				var userId = userPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

				if (string.IsNullOrEmpty(userId))
				{
					return Results.Unauthorized();
				}

				var cartProducts = await dbContext.CartProducts.Where(cp => cp.User.Id == userId).ToListAsync();
				var user = await dbContext.Users.FindAsync(userId);
				if (user == null)
				{
					return Results.NotFound();
				}
				if (!cartProducts.Any())
				{
					
					var newCart = new CartDTO(user.ToUserDTO(),new Dictionary<ProductDTO, int>());
					return Results.Ok(newCart);
				}
				return Results.Ok(cartProducts.Any() ? cartProducts.ToCartDTO() : new CartDTO(user.ToUserDTO(), new Dictionary<ProductDTO, int>()));

			}).RequireAuthorization();
			

			app.MapPost("/api/Cart", async (WebShopDbContext dbContext, CartDTO cartDto) =>
			{
				var cartProducts = cartDto.ToCartProducts();
				await dbContext.CartProducts.AddRangeAsync(cartProducts);
				return Results.Ok();

			}).RequireAuthorization();
		}
	}
}
	
	
