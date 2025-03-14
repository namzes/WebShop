using System.Net.Http.Json;
using System.Text.Json;
using Webshop.Shared.Models;


namespace WebShop.Shared.Services
{
	public class CartService
	{
		public CartDTO? Cart { get; set; }
		public int CartItemCount => Cart?.CartProductDtos.Sum(cp => cp.Quantity) ?? 0;
		public event Action<int>? OnCartUpdated;
		public async void InitializeCart(HttpClient httpClient)
		{
			var response = await httpClient.GetAsync("/api/GetCart");

			if (response.IsSuccessStatusCode)
			{
				var json = await response.Content.ReadAsStringAsync();
				var cartProductDtos = JsonSerializer.Deserialize<List<CartProductDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				Cart = new CartDTO
				{
					CartProductDtos = cartProductDtos ?? new List<CartProductDTO>()
				};
			}
		}

		public async Task<List<CartProductDisplayDTO>> GetCartProducts(HttpClient httpClient)
		{
			if (Cart == null)
			{
				return new List<CartProductDisplayDTO>();
			}
			var jsonContent = JsonSerializer.Serialize(Cart.CartProductDtos);
			Console.WriteLine("Serialized JSON content: " + jsonContent);
			var response = await httpClient.PostAsJsonAsync("/api/GetCartProducts", Cart.CartProductDtos);

			if (response.IsSuccessStatusCode)
			{
				var json = await response.Content.ReadAsStringAsync();
				var productDtos = JsonSerializer.Deserialize<List<ProductDTO>>(json,
					new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

				var cartProductDisplayDtos = new List<CartProductDisplayDTO>();
				if (Cart != null && productDtos != null)
				{
					foreach (var productDto in productDtos)
					{
						var quantity = Cart.CartProductDtos.FirstOrDefault(cpDto => cpDto.ProductId == productDto.Id)
							?.Quantity ?? 0;
						cartProductDisplayDtos.Add(new CartProductDisplayDTO
						{
							Id = productDto.Id,
							Name = productDto.Name,
							Description = productDto.Description,
							Price = productDto.Price,
							ImageUrl = productDto.ImageUrl,
							Quantity = quantity
						});
					}

					return cartProductDisplayDtos;
				}
			}

			return new List<CartProductDisplayDTO>();
		}

		public string AddToCart(ProductDTO productDto)
		{
			if (Cart == null)
			{
				return "Cart is not initialized.";
			}

			var cartProductDto = Cart.CartProductDtos.FirstOrDefault(cpDto => cpDto.ProductId == productDto.Id);

			if (cartProductDto == null)
			{
				Cart.CartProductDtos.Add(new CartProductDTO()
				{
					ProductId = productDto.Id,
					Quantity = 1
				});
				OnCartUpdated?.Invoke(Cart.CartProductDtos.Count);
				return "Item added";
			
			}

			if (cartProductDto.Quantity < productDto.Stock)
			{
				cartProductDto.Quantity++;
				OnCartUpdated?.Invoke(Cart.CartProductDtos.Count);
				return "Item qty increased";
				
			}

			return "Item out of stock";
		}
		public void RemoveFromCart(CartProductDTO product)
		{

			if (Cart != null)
			{
				Cart.CartProductDtos.Remove(product);
			}
		}
	}
}
