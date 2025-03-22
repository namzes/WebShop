using System.Net.Http;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Webshop.Shared.Models;
using WebShop.Client.Components.Pages;

namespace WebShop.Client.Components.Services
{
	public interface ICartRepository
	{
		public Task<CartDTO> GetCart();
		public Task SaveCart(CartDTO cart);
		public Task ClearCart();
	}
	public class LocalStorageCartRepository : ICartRepository
	{
		private readonly ILocalStorageService _localStorageService;
		
		public LocalStorageCartRepository(ILocalStorageService localStorageService)
		{
			_localStorageService = localStorageService;
		}
		public async Task<CartDTO> GetCart()
		{
			return await _localStorageService.GetItemAsync<CartDTO>("cart") ?? new();
		}
		public async Task SaveCart(CartDTO cart)
		{
			await _localStorageService.SetItemAsync("cart", cart);
		}

		public async Task ClearCart()
		{
			await _localStorageService.RemoveItemAsync("cart");
		}
	}
	public class BackEndCartRepository : ICartRepository
	{
		
		private readonly IHttpClientFactory _httpClientFactory;
		public BackEndCartRepository(IHttpClientFactory httpClientfactory)
		{
			_httpClientFactory = httpClientfactory;
		}
		public async Task<CartDTO> GetCart()
		{
			var client = _httpClientFactory.CreateClient("MinimalApi");
			var response = await client.GetAsync("/api/GetCart");

			if (response.IsSuccessStatusCode)
			{
				var json = await response.Content.ReadAsStringAsync();
				var cartProductDtos = JsonSerializer.Deserialize<List<CartProductDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				return new CartDTO
				{
					CartProductDtos = cartProductDtos ?? new List<CartProductDTO>()
				};
			}

			return new CartDTO();
		}

		public async Task SaveCart(CartDTO cart)
		{
				var client = _httpClientFactory.CreateClient("MinimalApi");
			    var json = JsonSerializer.Serialize(cart);
			    var content = new StringContent(json, Encoding.UTF8, "application/json");
			    
			    var response = await client.PostAsync("/api/Cart", content);

			    if (!response.IsSuccessStatusCode)
			    {
				    Console.WriteLine("Saving cart failed.");
			    }
		}

		public Task ClearCart()
		{
			return Task.FromResult("No logic yet");
		}
	}
	public class CartService
	{
		private CartDTO? _cart;
		public int CartItemCount => _cart?.CartProductDtos.Sum(cp => cp.Quantity) ?? 0;
		public event Action<int>? OnCartUpdated;
		private LocalStorageCartRepository _cartRepository;
		private BackEndCartRepository _backEndCartRepository;
		private IProductService _productService;

		public CartService(LocalStorageCartRepository cartRepository, BackEndCartRepository backEndCartRepository, IProductService productService)
		{
			_cartRepository = cartRepository;
			_productService = productService;
			_backEndCartRepository = backEndCartRepository;
		}
		public async Task<CartDTO> GetCart()
		{
			if (_cart != null)
			{
				return _cart;
			}
			_cart = await _cartRepository.GetCart();
			return _cart;
		}

		public async Task AddToCart(ProductDTO productDto)
		{
			_cart = await GetCart();

			var cartProductDto = _cart.CartProductDtos.FirstOrDefault(cpDto => cpDto.ProductId == productDto.Id);

			if (cartProductDto == null)
			{
				_cart.CartProductDtos.Add(new CartProductDTO()
				{
					ProductId = productDto.Id,
					Quantity = 1
				});
				OnCartUpdated?.Invoke(_cart.CartProductDtos.Count);
				
			}
			else if (cartProductDto.Quantity < productDto.Stock)
			{
				cartProductDto.Quantity++;
				OnCartUpdated?.Invoke(_cart.CartProductDtos.Count);

			}
			await _cartRepository.SaveCart(_cart);

		}

		public async Task RemoveFromCart(ProductDTO product)
		{

			_cart = await GetCart();

			var cartProductDto = _cart.CartProductDtos.FirstOrDefault(cpDto => cpDto.ProductId == product.Id);

			if (cartProductDto != null)
			{
				if (cartProductDto.Quantity > 1)
				{
					cartProductDto.Quantity --;
				}
				else
				{
					_cart.CartProductDtos.Remove(cartProductDto);
				}
			}
			OnCartUpdated?.Invoke(_cart.CartProductDtos.Count);
			await _cartRepository.SaveCart(_cart);
		}

		public async Task ClearCart()
		{
			await _cartRepository.ClearCart();
			_cart = new CartDTO();

		}
		public async Task<List<CartProductDisplayDTO>> GetCartProductDisplayDtos()
		{
			_cart = await GetCart();
			var productDtos = await _productService.GetProductsFromCartProducts(_cart.CartProductDtos);
			var cartProductDisplayDtos = productDtos
				.Select(product =>
				{
					var cartProduct = _cart.CartProductDtos.FirstOrDefault(cp => cp.ProductId == product.Id);
					if (cartProduct != null)
					{
						return product.ToCartProductDisplayDto(cartProduct);
					}
					return null; 
				})
				.Where(dto => dto != null)
				.Cast<CartProductDisplayDTO>()
				.ToList();

			return cartProductDisplayDtos;
		}
		public async Task SyncCartToDatabase()
		{
			_cart = await GetCart();
			await _backEndCartRepository.SaveCart(_cart); // Save to backend
		}
	}
}
