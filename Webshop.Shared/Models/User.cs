using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Webshop.Shared.Models
{
    public class User(string username, string password, string email, string address)
	{
        public int Id { get; set; }
		public required string Username { get; set; } = username;
		public required string Password { get; set; } = password;
		public required string Email { get; set; } = email;
		public required string Address { get; set; } = address;

		public Cart? Cart { get; set; } = new Cart();
	}
}
