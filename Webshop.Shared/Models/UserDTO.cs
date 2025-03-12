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
		public  string? Id { get; set; }
		public  string? Email { get; set; }
	}

	public class UserPostDTO
	{
		public required string Email { get; set; }
		public required string Password { get; set; }
		
	}
}
