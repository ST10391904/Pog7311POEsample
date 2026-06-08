using Microsoft.AspNetCore.Mvc;
using Prog7311PoeTwo.Models;
using System.Net.Http.Json;

namespace Prog7311PoeTwo.Controllers
{
    public class ClientController : Controller
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;

        public ClientController(IHttpClientFactory factory, IConfiguration config)
        {
            _http = factory.CreateClient();
            _baseUrl = $"{config["ApiSettings:BaseUrl"]}/api/clients";
        }

        public async Task<IActionResult> Index()
        {
            var clients = await _http.GetFromJsonAsync<List<ClientDetails>>(_baseUrl);
            return View(clients);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientDetails client)
        {
            if (!ModelState.IsValid)
                return View(client);

            var response = await _http.PostAsJsonAsync(_baseUrl, client);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Failed to create client.");
            return View(client);
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = await _http.GetFromJsonAsync<ClientDetails>($"{_baseUrl}/{id}");
            return client == null ? NotFound() : View(client);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = await _http.GetFromJsonAsync<ClientDetails>($"{_baseUrl}/{id}");
            return client == null ? NotFound() : View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClientDetails client)
        {
            if (!ModelState.IsValid)
                return View(client);

            var response = await _http.PutAsJsonAsync($"{_baseUrl}/{id}", client);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Failed to update client.");
            return View(client);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = await _http.GetFromJsonAsync<ClientDetails>($"{_baseUrl}/{id}");
            return client == null ? NotFound() : View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _http.DeleteAsync($"{_baseUrl}/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}