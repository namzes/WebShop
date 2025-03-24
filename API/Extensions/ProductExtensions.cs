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
				Stock = product.Stock,
				Sale = product.Sale != null ? product.Sale.ToSaleDTO() : new SaleDTO{IsOnSale = false, SaleRate = 1}


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
				Stock = product.Stock,
				Sale = product.Sale != null ? product.Sale.ToSaleDTO() : new SaleDTO { IsOnSale = false, SaleRate = 1 }
			};
			return productDTO;
		}
		public static SaleDTO ToSaleDTO(this Sale sale)
		{
			var saleDTO = new SaleDTO()
			{
				IsOnSale = sale.IsOnSale,
				SaleRate = sale.SaleRate
			};
			return saleDTO;
		}
	}
}
