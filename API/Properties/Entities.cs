using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Properties
{
	public class User : IdentityUser
	{
	}
	public class Product
	{
		public int Id { get; set; }
		[StringLength(200)]
		public required string Name { get; set; }
		[StringLength(200)]
		public required string Description { get; set; }
		public decimal Price { get; set; }
		[StringLength(200)]
		public required string ImageUrl { get; set; }
		public int Stock { get; set; }
		public Sale? Sale { get; set; }
	}
	public class Sale
	{
		public int Id { get; set; }
		public bool IsOnSale { get; set; }
		[Range(0, 1, ErrorMessage = "Sale rate must be between 0 and 1.")]
		public decimal SaleRate { get; set; }
	}

	public class CartProduct
	{
		public int Id { get; set; }
		public int Quantity { get; set; } = 1;
		public required User User { get; set; }
		public required Product Product { get; set; }
	}
	public class Order
	{
		public int Id { get; set; }
		public required User User { get; set; }
		[StringLength(200)]
		public required string ShippingAddress { get; set; }
		[StringLength(200)]
		public required string City { get; set; }
		public List<OrderedProduct>? OrderedProducts { get; set; }
	}

	public class OrderedProduct
	{
		public int Id { get; set; }
		public int Quantity { get; set; }
		public required Product Product { get; set; }
	}
}
