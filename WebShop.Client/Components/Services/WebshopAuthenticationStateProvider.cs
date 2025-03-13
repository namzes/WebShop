using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Webshop.Shared.Models;
using Microsoft.JSInterop;

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
			var client = _httpClientFactory.CreateClient("MinimalApi");
			try
			{
				var response = await client.GetAsync("/Account/me");

				if (response.IsSuccessStatusCode)
				{
					var json = await response.Content.ReadAsStringAsync();
					var user = JsonSerializer.Deserialize<UserGetDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


					if (user != null && !string.IsNullOrEmpty(user.Id) && !string.IsNullOrEmpty(user.Email))
					{
						var claims = new List<Claim>
						{
							new Claim(ClaimTypes.NameIdentifier, user.Id),
							new Claim(ClaimTypes.Name, user.Email),
						};
						var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
						var userPrincipal = new ClaimsPrincipal(identity);
						return new AuthenticationState(userPrincipal);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching user: {ex.Message}");
			}

			return GetAnonymousUser();
		}


		private static AuthenticationState GetAnonymousUser()
		{
			var anonymousIdentity = new ClaimsIdentity();
			var anonymousClaimsPrincipal = new ClaimsPrincipal(anonymousIdentity);

			return new AuthenticationState(anonymousClaimsPrincipal);
		}

		public async Task<HttpResponseMessage> SignInAsync(string email, string password)
		{
			var client = _httpClientFactory.CreateClient("MinimalApi");
			var response = new HttpResponseMessage();

			try
			{
				var json = JsonSerializer.Serialize(new { email, password });
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = await client.PostAsync("/Account/login?useCookies=true", content);

				if (response.IsSuccessStatusCode)
				{
					var authState = await GetAuthenticationStateAsync();
					NotifyAuthenticationStateChanged(Task.FromResult(authState));
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Login error: {ex.Message}");
			}

			return response;
		}

	}

}
