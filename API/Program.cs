using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Webshop.Shared.Models;
using WebShop.API.Extensions;
using WebShop.API.Properties;
using WebShop.API.Services;


namespace API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			

			builder.Services.AddAuthorization();
			builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
				.AddCookie(IdentityConstants.ApplicationScheme);

			builder.Services.AddIdentity<User, IdentityRole>()
				.AddEntityFrameworkStores<WebShopDbContext>()
				.AddDefaultTokenProviders();

			var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");

			builder.Services.AddDbContext<WebShopDbContext>(opt =>
				opt.UseSqlServer(connectionString).LogTo(Console.WriteLine, LogLevel.Information));

			var app = builder.Build();
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			
			app.MapEndpoints();
			app.MapGroup("/Account").MapIdentityApi<User>();
			
			app.Run();
		}

		
	}
}
