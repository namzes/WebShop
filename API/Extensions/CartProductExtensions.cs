using WebShop.API.Properties;
using Webshop.Shared.Models;

namespace WebShop.API.Extensions
{
	public static class CartProductExtensions
	{
		public static CartProductDTO ToCartProductDTO(this CartProduct cartProduct)
		{
			var cartProductDto = new CartProductDTO()
			{
				ProductId = cartProduct.Id,
				Quantity = cartProduct.Quantity,

			};
			return cartProductDto;
		}

		public static List<CartProductDTO> ToCartProductDTOList(this List<CartProduct> cartProducts)
		{
			var cartProductDtos = cartProducts.Select(cartProduct => cartProduct.ToCartProductDTO()).ToList();

			return cartProductDtos;
		}

		public static CartProduct ToCartProduct(this CartProductDTO cartProductDto, User user, Product product)
		{
			var cartProduct = new CartProduct()
			{
				Id = cartProductDto.ProductId,
				Quantity = cartProductDto.Quantity,
				User = user,
				Product = product
			};
			return cartProduct;
		}

		public static List<CartProduct> ToCartProducts(this List<CartProductDTO> cartProductDtos, List<Product> products, User user)
		{
			var cartProducts = cartProductDtos.Select(cpDto =>
				{
					var product = products.FirstOrDefault(p => p.Id == cpDto.ProductId);
					return product != null ? cpDto.ToCartProduct(user, product) : null;
				}).Where(cp => cp != null)
				.Cast<CartProduct>()
				.ToList();

			return cartProducts;
		}
	}

}
