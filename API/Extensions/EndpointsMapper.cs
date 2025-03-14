using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Webshop.Shared.Models;
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

				return Results.Ok(cartProducts.Any() ? cartProducts.ToCartProductDTOList() : new List<CartProductDTO>());
			}).RequireAuthorization();

			app.MapPost("/api/GetCartProducts",
				async (HttpContext httpContext, [FromServices] WebShopDbContext dbContext, [FromBody] List<CartProductDTO> cartProductDtos) =>
				{
					var userPrincipal = httpContext.User;
					var userId = userPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

					if (string.IsNullOrEmpty(userId))
					{
						return Results.Unauthorized();
					}

					var products = await dbContext.Products
						.Where(cp => cartProductDtos.Select(cpd => cpd.ProductId).Contains(cp.Id)).ToListAsync();

					return Results.Ok(products.ToProductDTOs());

				}).RequireAuthorization();

			app.MapPost("/api/Cart", async ( HttpContext httpContext,  WebShopDbContext dbContext, CartDTO cartDto) =>
			{
				var userId = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
				if (string.IsNullOrEmpty(userId))
				{
					return Results.Unauthorized();
				}
				var user = await dbContext.Users.FindAsync(userId);
				if (user == null)
				{
					return Results.NotFound("User not found.");
				}

				var productIds = cartDto.CartProductDtos.Select(p => p.ProductId).ToList();
				var products = await dbContext.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

				var cartProducts = cartDto.CartProductDtos.ToCartProducts(products, user);

				if (!cartProducts.Any())
				{
					return Results.BadRequest("No valid cart products found.");
				}

				await dbContext.CartProducts.AddRangeAsync(cartProducts);
				await dbContext.SaveChangesAsync();

				return Results.Ok();

			}).RequireAuthorization();
		}
	}
}
	
	
