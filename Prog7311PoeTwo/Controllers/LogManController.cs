using Microsoft.AspNetCore.Mvc;
using Prog7311PoeTwo.Models;
using System.Net.Http.Json;

namespace Prog7311PoeTwo.Controllers
{
    public class LogManController : Controller
    {
        private readonly HttpClient _httpClient;

        private const string API_URL = "http://localhost:5010/api/logman";

        public LogManController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        public async Task<IActionResult> Index(
            string? clientName,
            ContractStatus? status,
            DateOnly? startDate,
            DateOnly? endDate)
        {
            var url = $"{API_URL}?clientName={clientName}&status={status}&startDate={startDate}&endDate={endDate}";

            var result = await _httpClient
                .GetFromJsonAsync<List<LogisticsManager>>(url);

            ViewBag.ClientName = clientName;
            ViewBag.Status = status;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View(result);
        }

        public async Task<IActionResult> RequestSLA(int id)
        {
            var response = await _httpClient.GetAsync($"{API_URL}/sla/{id}");

            if (!response.IsSuccessStatusCode)
                return BadRequest();

            var message = await response.Content.ReadAsStringAsync();

            return Content(message);
        }
    }
}