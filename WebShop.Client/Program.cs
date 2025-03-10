using System.Net;
using WebShop.Client.Components;
namespace WebShop.Client;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddCascadingAuthenticationState();

		builder.Services.AddHttpClient("MinimalApi",client => client.BaseAddress = new Uri("https://localhost:7119"))
	        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
        {
	        UseCookies = true, //Ensure cookies are used
	        CookieContainer = new CookieContainer()
        }); ;
		
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

        app.Run();
    }
}
