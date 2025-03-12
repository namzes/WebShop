using WebShop.API.Properties;
using Webshop.Shared.Models;

namespace WebShop.API.Extensions
{
	public static class CartProductExtensions
	{
		public static List<CartProduct> ToCartProducts(this CartDTO cartProductDtos)
		{
			var cartProducts = cartProductDtos.ProductsInCart
				.Select(productEntry => new CartProduct
				{
					Quantity = productEntry.Value,
					Product = productEntry.Key.ToProduct(),
					User = cartProductDtos.User.ToUser()
				}).ToList();

			return cartProducts;
		}
		public static CartDTO ToCartDTO(this List<CartProduct> cartProducts)
		{
			// Get the user from the first CartProduct (assuming all CartProducts in the list are for the same user)
			var user = cartProducts.FirstOrDefault()?.User;
			if (user == null)
			{
				throw new InvalidOperationException("Cart does not have a valid user.");
			}

			// Group the CartProducts by Product
			var productsInCart = cartProducts
				.GroupBy(cp => cp.Product)
				.ToDictionary(
					group => group.Key.ToProductDTO(),  // Convert each product to a ProductDTO
					group => group.Sum(cp => cp.Quantity)  // Sum quantities for each product
				);

			// Return the CartDTO with the User and the products in the cart
			return new CartDTO(user.ToUserDTO(), productsInCart);
		}
	}
}
