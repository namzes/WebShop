using System.Text.Json;
using Webshop.Shared.Models;

namespace WebShop.Client.Components.Services
{
	public class CartService
	{
		private readonly HttpClient _httpClient;
		public CartDTO? Cart { get; set; }

		public CartService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			
		}

		public async void InitializeCart()
		{
			var response = await _httpClient.GetAsync("/api/GetCart");

			if (response.IsSuccessStatusCode)
			{
				var json = await response.Content.ReadAsStringAsync();
				Cart = JsonSerializer.Deserialize<CartDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new CartDTO(new UserGetDTO() { Id = "", Email = "guest@example.com" }, new Dictionary<ProductDTO, int>()); ;
			}
		}

		public int GetCartItems()
		{
			if (Cart == null)
			{
				return 0;
			}
			return Cart.CartQuantity;
		}


		public void AddToCart(ProductDTO productDto)
		{
			if (Cart == null)
			{
				return;
			}

			if (Cart.ProductsInCart.ContainsKey(productDto))
			{
				Cart.ProductsInCart[productDto]++; 
			}
			else
			{
				Cart.ProductsInCart[productDto] = 1;
			}
		}
	}
}
