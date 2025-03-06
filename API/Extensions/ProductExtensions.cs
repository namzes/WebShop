using WebShop.API.Properties;
using Webshop.Shared.Models;

namespace WebShop.API.Extensions
{
	public static class ProductExtensions
	{
		public static List<ProductDTO> ToProductDTOs(this List<Product> products)
		{
			var productDTOs = products.Select(product => new ProductDTO()
			{
				Id = product.Id,
				Name = product.Name,
				Description = product.Description,
				Price = product.Price,
				ImageUrl = product.ImageUrl,
				Stock = product.Stock
			}).ToList();
			return productDTOs;
		}
		public static ProductDTO ToProductDTO(this Product product)
		{
			var productDTO = new ProductDTO()
			{
				Id = product.Id,
				Name = product.Name,
				Description = product.Description,
				Price = product.Price,
				ImageUrl = product.ImageUrl,
				Stock = product.Stock
			};
			return productDTO;
		}
	}
}
