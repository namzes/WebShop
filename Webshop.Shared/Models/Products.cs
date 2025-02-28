using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Shared.Models
{
    class Product
    {
		public int Id { get; set; }
		public required string Name { get; set; }
		public required string Description { get; set; }
		public decimal Price { get; set; }
		public required string ImageUrl { get; set; }
		public int Stock { get; set; }
	}

    public class CartProduct
    {
		public int ProductId { get; set; }
		public required string Name { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		
		public CartProduct(int productId, string name, decimal price, int quantity)
		{
			if (productId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(quantity), "Invalid product ID");
			}

			if (quantity < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be at least 1");
			}
			ProductId = productId;
			Name = name;
			Price = price;
			Quantity = quantity;
		}

	}
}
