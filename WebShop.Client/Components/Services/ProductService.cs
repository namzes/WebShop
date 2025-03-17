using System.Net.Http;
using System.Text.Json;
using Webshop.Shared.Models;

namespace WebShop.Client.Components.Services
{
	public interface IProductService
	{
		public Task<List<ProductDTO>> GetProducts();
		public Task<List<ProductDTO>> GetProductsFromCartProducts(List<CartProductDTO> cartProductDisplayDtos);
	}
	public class ApiProductService : IProductService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		public ApiProductService(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<List<ProductDTO>> GetProducts()
		{
			var client = _httpClientFactory.CreateClient("MinimalApi");
			var response = await client.GetFromJsonAsync<List<ProductDTO>>($"/api/Products");
			if (response == null)
			{
				return new List<ProductDTO>();
			}
			return response;
		}
		public async Task<List<ProductDTO>> GetProductsFromCartProducts(List<CartProductDTO> cartProductDisplayDtos)
		{
			var client = _httpClientFactory.CreateClient("MinimalApi");
			var response = await client.PostAsJsonAsync("/api/GetProductsFromCartProducts", cartProductDisplayDtos);
			if (response.IsSuccessStatusCode)
			{
				var json = await response.Content.ReadAsStringAsync();
				var products = JsonSerializer.Deserialize<List<ProductDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				return products ?? new List<ProductDTO>();
			}
			return new List<ProductDTO>();
		}
	}
	
}
