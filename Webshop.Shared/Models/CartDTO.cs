namespace Webshop.Shared.Models
{
	public class CartDTO
	{
		public List<CartProductDTO> CartProductDtos { get; set; } = new();
		public int UserId { get; set; }
	}

	public class CartProductDTO
	{
		public int ProductId;
		public int Quantity;
	}
}
