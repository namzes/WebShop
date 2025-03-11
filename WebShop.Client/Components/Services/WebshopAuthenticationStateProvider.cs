using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Webshop.Shared.Models;

namespace WebShop.Client.Components.Services
{
	public class WebshopAuthenticationStateProvider : AuthenticationStateProvider
	{
		private AuthenticationState _authState { get; set; }
		private IHttpClientFactory _httpClientFactory;
		public WebshopAuthenticationStateProvider(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
			_authState = new AuthenticationState(new ClaimsPrincipal());
		}
		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var httpClient = _httpClientFactory.CreateClient("MinimalApi");
			try
			{

				var userDto = await httpClient.GetFromJsonAsync<UserGetDTO>("/Account/me");


				if (userDto == null)
				{
					return GetAnonymousUser();
				}

				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, userDto.Email),
					new Claim(ClaimTypes.NameIdentifier, userDto.Id)
				};

				var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);

				var claimsPrincipal = new ClaimsPrincipal(identity);

				return new AuthenticationState(claimsPrincipal);
			}
			catch (Exception ex)
			{
				// Handle any errors during the fetch process and return an empty ClaimsPrincipal
				Console.WriteLine($"Error: {ex.Message}");
				return GetAnonymousUser();
			}
		}
		private static AuthenticationState GetAnonymousUser()
		{
			var anonymousIdentity = new ClaimsIdentity();
			var anonymousClaimsPrincipal = new ClaimsPrincipal(anonymousIdentity);

			return new AuthenticationState(anonymousClaimsPrincipal);
		}

		public async Task<bool> SignInAsync(string email, string password)
		{
			var content = new StringContent(JsonSerializer.Serialize(new { email, password }), Encoding.UTF8, "application/json");
			var client = _httpClientFactory.CreateClient("MinimalApi");

			var response = await client.PostAsync("/Account/login?useCookies=true", content);

			if (response.IsSuccessStatusCode) // 200 OK
			{
				// After successful login, create a ClaimsIdentity
				var identity = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.Name, email),
				}, IdentityConstants.ApplicationScheme);

				var claimsPrincipal = new ClaimsPrincipal(identity);

				_authState = new AuthenticationState(claimsPrincipal);

				NotifyAuthenticationStateChanged(Task.FromResult(_authState));
				return true;
			}

			return false;
		}
	}

}
