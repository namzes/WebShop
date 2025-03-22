using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Shared.Models
{
    public class ShippingDetailsDTO
    {
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public required string Address { get; set; }
		public required string City { get; set; }
		
	}
}
