using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddAuthorization();
			builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddCookie(IdentityConstants.ApplicationScheme);
			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.Cookie.Name = ".AspNetCore.Identity.Application";
				options.Cookie.HttpOnly = true;
				options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
				options.Cookie.SameSite = SameSiteMode.None;
				options.ExpireTimeSpan = TimeSpan.FromHours(7);
				options.SlidingExpiration = true;
				options.LoginPath = "/Account/login";
				options.AccessDeniedPath = "/Account/AccessDenied";
			});
			builder.Services.AddIdentityCore<User>()
				.AddEntityFrameworkStores<WebShopDbContext>()
				.AddDefaultTokenProviders().AddApiEndpoints();

			var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");

			builder.Services.AddDbContext<WebShopDbContext>(opt =>
				opt.UseSqlServer(connectionString).LogTo(Console.WriteLine, LogLevel.Information));

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowBlazorClient", policy =>
				{
					policy.WithOrigins("https://localhost:7191") // Use your Blazor app's origin
						.AllowCredentials() // Allow cookies
						.AllowAnyMethod()
						.AllowAnyHeader();
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

			app.UseCors("AllowBlazorClient");

			app.Run();
		}


	}
}
