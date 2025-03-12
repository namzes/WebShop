using WebShop.API.Properties;
using Webshop.Shared.Models;

namespace WebShop.API.Extensions
{
	public static class UserExtensions
	{
		public static  User ToUser(this UserGetDTO userDto)
		{
			return new User()
			{
				Id = userDto.Id ?? "",
				Email = userDto.Email ?? "guest.example.com",
			};
		}
		public static UserGetDTO ToUserDTO(this User user)
		{
			return new UserGetDTO()
			{
				Id = user.Id,
				Email = user.Email ?? "default@email.com"
			};
		}
	}
}
