namespace Webshop.Shared.Models
{
	public class CartDTO
	{
		public CartDTO (UserGetDTO user, Dictionary<ProductDTO, int> products)
		{
			ProductsInCart = products;
			User = user;
		}
		public UserGetDTO User { get; set; }
		public Dictionary<ProductDTO, int> ProductsInCart { get; set; }
		public int CartQuantity => ProductsInCart.Count;
	}
}
