using System.Text.Json;
using Webshop.Shared.Models;

namespace WebShop.Shared.Services
{
	public class CartService
	{
		public CartDTO? Cart { get; set; }
		public int CartItemCount => Cart?.CartProductDtos.Count ?? 0;
		public event Action<int>? OnCartUpdated;
		public async void InitializeCart(HttpClient httpClient)
		{
			var response = await httpClient.GetAsync("/api/GetCart");

			if (response.IsSuccessStatusCode)
			{
				var json = await response.Content.ReadAsStringAsync();
				var cartProductDtos = JsonSerializer.Deserialize<List<CartProductDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				Cart = new CartDTO()
				{
					CartProductDtos = cartProductDtos ?? new List<CartProductDTO>()
				};
			}
			
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
