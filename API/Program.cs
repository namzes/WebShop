using Microsoft.EntityFrameworkCore;
using Webshop.Shared.Models;
using WebShop.API.Extensions;
using WebShop.API.Properties;


namespace API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");
			builder.Services.AddDbContext<WebShopDbContext>(opt =>
				opt.UseSqlServer(connectionString).LogTo(Console.WriteLine, LogLevel.Information));

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowBlazor",
					policy => policy.WithOrigins("https://localhost:7191") // Update this to match your frontend URL
						.AllowAnyMethod()
						.AllowAnyHeader());
			});

			var app = builder.Build();
			
			app.MapEndpoints();

			app.UseCors("AllowBlazor");
			app.Run();
		}

		
	}
}
