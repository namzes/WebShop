using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Webshop.Shared.Models
{
	public class CartProduct
	{
		public int Id { get; set; }
		public int Quantity { get; set; }
		public required Product Product { get; set; }
	}
}
