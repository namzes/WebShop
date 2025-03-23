using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Blazored.LocalStorage;
using WebShop.Client.Components;
using WebShop.Client.Components.Services;

namespace WebShop.Client;
public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddRazorComponents()
			.AddInteractiveServerComponents();

		builder.Services.AddServerSideBlazor()
			.AddHubOptions(options =>
			{
				options.ClientTimeoutInterval = TimeSpan.FromMinutes(6);
				options.HandshakeTimeout = TimeSpan.FromMinutes(1); 
				options.KeepAliveInterval = TimeSpan.FromSeconds(10);
			});

		builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
			.AddCookie(IdentityConstants.ApplicationScheme, options =>
			{
				options.LoginPath = "/Account/login";
				options.LogoutPath = "/Account/logout";
				options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
				options.SlidingExpiration = true;
				options.Cookie.HttpOnly = true;
				options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
			});
		builder.Services.AddAuthorization();

		builder.Services.AddCascadingAuthenticationState();
		builder.Services.AddHttpContextAccessor();

		builder.Services.AddHttpClient("MinimalApi", (sp, client) => {
				var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
				client.BaseAddress = new Uri("https://localhost:7119");
			var cookieHeader = httpContextAccessor.HttpContext?.Request.Headers["Cookie"];
			if (!string.IsNullOrEmpty(cookieHeader))
			{
				client.DefaultRequestHeaders.Add("Cookie", cookieHeader.ToString());
			}

		})
			.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
			{
				UseCookies = true,
				CookieContainer = new CookieContainer()
			});
		
		builder.Services.AddScoped<WebshopAuthenticationStateProvider>();
		builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<WebshopAuthenticationStateProvider>());
		builder.Services.AddScoped<LocalStorageCartRepository>();
		builder.Services.AddScoped<BackEndCartRepository>();
		builder.Services.AddScoped<IProductService, ApiProductService>();
		builder.Services.AddScoped<IOrderService, OrderService>();
		builder.Services.AddScoped<CartService>();
		builder.Services.AddScoped<CurrencyService>();
		

		builder.Services.AddBlazoredLocalStorage();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseHttpsRedirection();

		app.UseAntiforgery();

		app.MapStaticAssets();
		app.MapRazorComponents<App>()
			.AddInteractiveServerRenderMode();

		app.UseAuthentication();
		app.UseAuthorization();

		app.Run();
	}
}
