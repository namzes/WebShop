namespace Webshop.Shared.Models
{
	public class CartDTO
	{
		public CartDTO(UserGetDTO user, Dictionary<ProductDTO, int> products)
		{
			Id = 21384532; //Some fake íd
			User = user;
			ProductsInCart = products;

		}
		public int Id { get; set; }
		public required UserGetDTO User { get; set; }
		public Dictionary<ProductDTO, int> ProductsInCart { get; set; } = new();
		public int CartQuantity => ProductsInCart.Count;
	}
}
