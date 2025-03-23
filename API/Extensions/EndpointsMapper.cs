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

			app.MapPost("/api/GetProductsFromCartProducts",
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

			app.MapPost("/api/Cart", async (HttpContext httpContext, WebShopDbContext dbContext, CartDTO cartDto) =>
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
					return Results.BadRequest("No valid cart products created.");
				}

				foreach (var cartProduct in cartProducts)
				{
					var existingCartProduct = await dbContext.CartProducts
						.FirstOrDefaultAsync(cp => cp.Product.Id == cartProduct.Product.Id && cp.User.Id == user.Id);

					if (existingCartProduct != null)
					{
						existingCartProduct.Quantity += cartProduct.Quantity;
						dbContext.CartProducts.Update(existingCartProduct);
					}
					else
					{
						await dbContext.CartProducts.AddAsync(cartProduct);
					}
				}
				await dbContext.SaveChangesAsync();

				return Results.Ok();

			}).RequireAuthorization();

			app.MapPost("/api/Order", async (HttpContext httpContext, [FromServices] WebShopDbContext dbContext, [FromBody] OrderRequestDTO orderRequest) =>
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
				var cartProducts = await dbContext.CartProducts.Where(cp => cp.User.Id == user.Id).Include(cp => cp.Product).ToListAsync();

				if (!cartProducts.Any())
				{
					return Results.BadRequest("No valid cart products created.");
				}

				var productIds = cartProducts.Select(cp => cp.Id).ToList();
				var products = await dbContext.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

				foreach (var product in products)
				{
					var cartProduct = cartProducts.FirstOrDefault(cp => cp.Product.Id == product.Id);
					if (cartProduct != null)
					{
						product.Stock -= cartProduct.Quantity;
					}
				}

				dbContext.CartProducts.RemoveRange(cartProducts);

				var order = new Order()
				{
					User = user,
					ShippingAddress = orderRequest.ShippingDetails.Address,
					City = orderRequest.ShippingDetails.City,
					OrderedProducts = cartProducts.ToOrderedProducts()
				};

				await dbContext.Orders.AddAsync(order);
				await dbContext.SaveChangesAsync();
				return Results.Ok();
			}).RequireAuthorization();

			app.MapGet("/api/ExchangeRate/{currencyPair}", async (string currencyPair, [FromServices] IHttpClientFactory httpClientFactory) =>
			{
				string[] currencies = currencyPair.Split('_');
				string fromCurrency = currencies[0];
				string toCurrency = currencies[1];
				string? apiKey = app.Configuration.GetValue<string>("ApiSettings:ApiKey");
				var apiUrl = $"https://v6.exchangerate-api.com/v6/{apiKey}/pair/{fromCurrency}/{toCurrency}";

				if (apiKey == null)
				{
					return Results.BadRequest("Api key not found.");
				}

				using (var client = httpClientFactory.CreateClient())
				{
					try
					{
						var response = await client.GetStringAsync(apiUrl);
						return Results.Ok(response);
					}
					catch (Exception ex)
					{
						return Results.Problem($"An error occurred: {ex.Message}", statusCode: 500);
					}
				}
			});
		}
	}
}



