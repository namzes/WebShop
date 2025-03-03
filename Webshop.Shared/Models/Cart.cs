namespace Webshop.Shared.Models
{
	public class Cart
	{
		public List<CartProduct> CartProducts { get; set; } = new();

		public int CartQuantity => CartProducts.Count;
	}
}
