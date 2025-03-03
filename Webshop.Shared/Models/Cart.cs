namespace Webshop.Shared.Models
{
	public class Cart
	{
		public int Id { get; set; }
		public List<CartProduct> CartProducts { get; set; } = new();
		public int CartQuantity => CartProducts.Count;
	}
}
