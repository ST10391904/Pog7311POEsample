using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace PROG7311POEAPI.Services
{
    public class ExchangeAPI : ICurrency
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public ExchangeAPI(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task<decimal> ConvertToZAR(string fromCurrency, decimal amount)
        {
            var apiKey = _config["CurrencyApi:ApiKey"];

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new Exception("Missing Currency API Key");

            var url = $"https://openexchangerates.org/api/latest.json?app_id={apiKey}";

            var response = await _http.GetFromJsonAsync<ExchangeResponse>(url);

            if (response?.Rates == null || response.Rates.Count == 0)
                throw new Exception("Failed to retrieve exchange rates from API");

            var rates = response.Rates;

            fromCurrency = fromCurrency.ToUpper().Trim();

            if (!rates.TryGetValue(fromCurrency, out var fromRate))
                throw new Exception($"Currency not supported: {fromCurrency}");

            if (!rates.TryGetValue("ZAR", out var zarRate))
                throw new Exception("ZAR rate missing from API response");

            decimal usdAmount = fromCurrency == "USD"
                ? amount
                : amount / fromRate;

            return usdAmount * zarRate;
        }

        private class ExchangeResponse
        {
            public Dictionary<string, decimal> Rates { get; set; } = new();
        }
    }
}