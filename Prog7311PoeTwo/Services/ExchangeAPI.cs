using System.Text.Json;

namespace Prog7311PoeTwo.Services
{
    public class ExchangeAPI : ICurrency
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<decimal> ConvertToZAR(string fromCurrency, decimal amount)
        {
            string apiKey = "0dfd90986fb647fe9bcf0b26af2320da";
            string url = $"https://openexchangerates.org/api/latest.json?app_id={apiKey}";

            var response = await _httpClient.GetStringAsync(url);

            var json = JsonDocument.Parse(response);

            var rates = json.RootElement.GetProperty("rates");

            decimal zarRate = rates.GetProperty("ZAR").GetDecimal();

            decimal finalAmount;

            if (fromCurrency == "USD")
            {
                finalAmount = amount * zarRate;
            }
            else
            {
                decimal fromRate = rates.GetProperty(fromCurrency).GetDecimal();

                decimal amountInUSD = amount / fromRate;

                finalAmount = amountInUSD * zarRate;
            }

            return finalAmount;
        }
    }
}