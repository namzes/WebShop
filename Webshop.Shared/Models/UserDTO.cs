using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace Webshop.Shared.Models
{

	public class UserGetDTO
	{
		public required string Id { get; set; }
		public required string Email { get; set; }
		public required string Username { get; set; }
	}

	public class UserPostDTO
	{
		public required string Email { get; set; }
		public required string Password { get; set; }
		
	}
}
