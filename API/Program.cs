using Webshop.Shared.Models;
using WebShop.API.Extensions;
namespace API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var app = builder.Build();

			app.MapEndpoints();

			app.Run();
		}

		
	}
}
