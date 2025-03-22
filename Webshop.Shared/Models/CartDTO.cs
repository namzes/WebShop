namespace Webshop.Shared.Models
{
	public class CartDTO
	{
		public List<CartProductDTO> CartProductDtos { get; set; } = new();
	}

	public class CartProductDTO
	{
		public int ProductId { get; set; }
		public int Quantity { get; set; }

	}
	public class CartProductDisplayDTO
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public required string Description { get; set; }
		public decimal Price { get; set; }
		public required string ImageUrl { get; set; }
		public int Stock { get; set; }
		public int Quantity { get; set; }
	}

	public static class CartProductExtensions
	{
		public static CartProductDisplayDTO ToCartProductDisplayDto(this ProductDTO productDto, CartProductDTO cartProductDto)
		{
			return new CartProductDisplayDTO
			{
				Id = productDto.Id,
				Name = productDto.Name,
				Description = productDto.Description,
				Price = productDto.Price,
				ImageUrl = productDto.ImageUrl,
				Quantity = cartProductDto.Quantity,
				Stock = productDto.Stock

			};
		}

		public static ProductDTO ToProductDto(this CartProductDisplayDTO cartProductDisplayDto)
		{
			return new ProductDTO()
			{
				Id = cartProductDisplayDto.Id,
				Name = cartProductDisplayDto.Name,
				Description = cartProductDisplayDto.Description,
				Price = cartProductDisplayDto.Price,
				ImageUrl = cartProductDisplayDto.ImageUrl,
				Stock = cartProductDisplayDto.Stock
			};
		}
	}
}
