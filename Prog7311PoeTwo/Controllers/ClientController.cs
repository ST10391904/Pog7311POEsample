using Microsoft.AspNetCore.Mvc;
using Prog7311PoeTwo.Models;
using System.Net.Http.Json;

namespace Prog7311PoeTwo.Controllers
{
    public class ClientController : Controller
    {
        private readonly HttpClient _http;

        public ClientController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient();
        }

        private string baseUrl = "https://localhost:5001/api/clients";

        public async Task<IActionResult> Index()
        {
            var clients = await _http.GetFromJsonAsync<List<ClientDetails>>(baseUrl);
            return View(clients);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClientDetails client)
        {
            var response = await _http.PostAsJsonAsync(baseUrl, client);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return View(client);
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = await _http.GetFromJsonAsync<ClientDetails>($"{baseUrl}/{id}");

            if (client == null)
                return NotFound();

            return View(client);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = await _http.GetFromJsonAsync<ClientDetails>($"{baseUrl}/{id}");

            if (client == null)
                return NotFound();

            return View(client);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ClientDetails client)
        {
            var response = await _http.PutAsJsonAsync($"{baseUrl}/{id}", client);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return View(client);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = await _http.GetFromJsonAsync<ClientDetails>($"{baseUrl}/{id}");
            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _http.DeleteAsync($"{baseUrl}/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}