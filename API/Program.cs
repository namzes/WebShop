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


			builder.Services.AddIdentityCore<User>()
				.AddEntityFrameworkStores<WebShopDbContext>()
				.AddDefaultTokenProviders().AddApiEndpoints();

			builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddCookie(IdentityConstants.ApplicationScheme);
			builder.Services.AddAuthorization();


			var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");

			builder.Services.AddDbContext<WebShopDbContext>(opt =>
				opt.UseSqlServer(connectionString).LogTo(Console.WriteLine, LogLevel.Information));

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowBlazorApp", policy =>
				{
					policy.WithOrigins("https://localhost:7191")  // Adjust to your Blazor app's URL
						.AllowAnyHeader()
						.AllowAnyMethod()
						.AllowCredentials();  // Allow credentials (cookies) to be sent
				});
			});

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseAuthentication();
			app.UseAuthorization();

			app.MapEndpoints();
			app.MapGroup("/Account").MapIdentityApi<User>();

			app.UseCors("AllowBlazorApp");

			app.Run();
		}


	}
}
