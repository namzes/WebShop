using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Blazored.LocalStorage;
using WebShop.Client.Components;
using WebShop.Client.Components.Services;
using WebShop.Shared.Services;

namespace WebShop.Client;
public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddRazorComponents()
			.AddInteractiveServerComponents();

		builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
			.AddCookie(IdentityConstants.ApplicationScheme);
		builder.Services.AddAuthorization();

		builder.Services.AddCascadingAuthenticationState();
		builder.Services.AddHttpClient("MinimalApi", client => client.BaseAddress = new Uri("https://localhost:7119"))
			.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
			{ 
				UseCookies = true,
				CookieContainer = new CookieContainer()
			});
		builder.Services.AddHttpContextAccessor();
		builder.Services.AddScoped<WebshopAuthenticationStateProvider>();
		builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<WebshopAuthenticationStateProvider>());
		builder.Services.AddSingleton<CartService>();

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
