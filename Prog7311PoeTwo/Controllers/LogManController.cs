using Microsoft.AspNetCore.Mvc;
using Prog7311PoeTwo.Models;
using System.Net.Http.Json;

namespace Prog7311PoeTwo.Controllers
{
    public class LogManController : Controller
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;

        public LogManController(IHttpClientFactory factory, IConfiguration config)
        {
            _http = factory.CreateClient();
            _baseUrl = $"{config["ApiSettings:BaseUrl"]}/api/logman";
        }

        public async Task<IActionResult> Index(
            string? clientName,
            ContractStatus? status,
            DateOnly? startDate,
            DateOnly? endDate)
        {
            var url = $"{_baseUrl}?clientName={clientName}&status={status}&startDate={startDate}&endDate={endDate}";

            var data = await _http.GetFromJsonAsync<List<LogisticsManager>>(url);
            return View(data);
        }

        public async Task<IActionResult> RequestSLA(int id)
        {
            var response = await _http.GetAsync($"{_baseUrl}/sla/{id}");

            if (!response.IsSuccessStatusCode)
                return BadRequest("Unable to request SLA");

            return Content(await response.Content.ReadAsStringAsync());
        }
    }
}