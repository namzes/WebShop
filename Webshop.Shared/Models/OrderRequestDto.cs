
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Shared.Models
{
    public class OrderRequestDTO
    {
        public required CartDTO Cart { get; set; }
		public required ShippingDetailsDTO ShippingDetails { get; set; }
	}
}
