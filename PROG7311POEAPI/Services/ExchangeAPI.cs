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

        public async Task<decimal> ConvertToZAR(string currency, decimal amount)
        {
            var baseUrl = _config["CurrencyApi:BaseUrl"];

            if (string.IsNullOrWhiteSpace(baseUrl))
                return 0;

            var response = await _http.GetFromJsonAsync<ExchangeResponse>(
                $"{baseUrl}{currency}"
            );

            if (response == null || response.Rates == null)
                return 0;

            if (!response.Rates.ContainsKey("ZAR"))
                return 0;

            var rate = response.Rates["ZAR"];

            return amount * rate;
        }

        private class ExchangeResponse
        {
            public Dictionary<string, decimal> Rates { get; set; }
        }
    }
}