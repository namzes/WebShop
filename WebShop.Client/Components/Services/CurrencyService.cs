using System.Text.Json;
using Webshop.Shared.Models;

namespace WebShop.Client.Components.Services
{
	public class CurrencyService
	{
		private string _selectedCurrency = "SEK";
		public string SelectedCulture { get; private set; } = "sv-SE";
		private decimal _exchangerate = 1;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly Dictionary<string, string> _currencyCultures = new()
		{
			{ "SEK", "sv-SE" },
			{ "USD", "en-US" },
			{ "EUR", "de-DE" },
			{ "GBP", "en-GB" },
			{ "AUD", "en-AU" },
			{ "CAD", "en-CA" },
			{ "JPY", "ja-JP" },
			{ "CHF", "fr-CH" },
			{ "NOK", "nb-NO" },
			{ "DKK", "da-DK" },
			{ "INR", "hi-IN" },
			{ "CNY", "zh-CN" },
			{ "MXN", "es-MX" }
		};

		public CurrencyService(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}
		public string SelectedCurrency
		{
			get => _selectedCurrency;
			set
			{
				if (_selectedCurrency != value)
				{
					_selectedCurrency = value;
					OnCurrencyChanged?.Invoke(_selectedCurrency);
					_ = GetExchangeRateAsync(); 
				}
			}
		}
		public decimal ExchangeRate => _exchangerate;
		public event Action<string>? OnCurrencyChanged;

		public async Task GetExchangeRateAsync()
		{
			var baseCurrency = "SEK";
			var currencyPair = $"{baseCurrency}_{_selectedCurrency}";

			try
			{
				var client = _httpClientFactory.CreateClient("MinimalApi");
				var response = await client.GetStringAsync($"/api/ExchangeRate/{currencyPair}");
				var unescapedResponse = JsonSerializer.Deserialize<string>(response);
				if (unescapedResponse != null)
				{
					var exchangeRateResponse = JsonSerializer.Deserialize<ExchangeRateResponse>(unescapedResponse);
					if (exchangeRateResponse != null)
					{
						_exchangerate = exchangeRateResponse.ConversionRate;
						
					}
					else
					{
						_exchangerate = 1;
					}
				}

			}
				
			catch (Exception)
			{
				_exchangerate = 1;
			}
		}
		public void SetCurrency(string currency)
		{
			if (_currencyCultures.TryGetValue(currency, out var culture))
			{
				SelectedCurrency = currency;
				SelectedCulture = culture;
				OnCurrencyChanged?.Invoke(currency);
			}
		}
	}
}
