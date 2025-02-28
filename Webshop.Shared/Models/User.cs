using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Shared.Models
{
    class User
    {
        public int Id { get; set; }
		public required string Username { get; set; }
		public required string Password { get; set; }
		public required string Email { get; set; }
		public required string Address { get; set; }

		public List<CartProduct>? Product { get; set; }
	}
}
