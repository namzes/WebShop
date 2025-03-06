using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();


			builder.Services.AddAuthorization();

			builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddCookie(IdentityConstants.ApplicationScheme);

			builder.Services.AddIdentityCore<User>()
				.AddEntityFrameworkStores<WebShopDbContext>()
				.AddDefaultTokenProviders().AddApiEndpoints();


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
