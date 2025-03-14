﻿using Microsoft.AspNetCore.Identity;
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
		public List<CartProduct> OrderedProducts { get; set; } = new();
	}
}
