using WebShop.Client.Components.Pages;
using Webshop.Shared.Models;

namespace WebShop.Client.Components.Services
{
	public interface IOrderService
	{
		public Task<bool> PlaceOrder(ShippingDetailsDTO shippingDetails);
		public OrderRequestDTO CreateOrderRequest(CartDTO cart, ShippingDetailsDTO shippingDetails);
	}
	public class OrderService : IOrderService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly CartService _cartService;
		private readonly LocalStorageCartRepository _cartRepository;

		public OrderService(IHttpClientFactory httpClientFactory, CartService cartService, LocalStorageCartRepository cartRepository)
		{
			_httpClientFactory = httpClientFactory;
			_cartService = cartService;
			_cartRepository = cartRepository;
		}
		public async Task<bool> PlaceOrder(ShippingDetailsDTO shippingDetails)
		{
			var cart = await _cartService.GetCart();
			var orderRequest = CreateOrderRequest(cart, shippingDetails);

			var client = _httpClientFactory.CreateClient("MinimalApi");
			var response = await client.PostAsJsonAsync("/api/Order", orderRequest);
			if (response.IsSuccessStatusCode)
			{
				await _cartRepository.ClearCart();
			}
			return response.IsSuccessStatusCode;
		}

		public OrderRequestDTO CreateOrderRequest(CartDTO cart, ShippingDetailsDTO shippingDetails)
		{
			return new OrderRequestDTO()
			{
				Cart = cart,
				ShippingDetails = shippingDetails
			};
		}
	}
}
