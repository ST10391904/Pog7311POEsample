using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Prog7311PoeTwo.Models;
using System.Net.Http.Json;

namespace Prog7311PoeTwo.Controllers
{
    public class ContractsController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly HttpClient _httpClient;

        private const string API_URL = "http://localhost:5010/api/contracts";

        public ContractsController(
            IWebHostEnvironment environment,
            IHttpClientFactory httpClientFactory)
        {
            _environment = environment;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            var contracts = await _httpClient.GetFromJsonAsync<List<Contracts>>(API_URL);
            return View(contracts);
        }

        public IActionResult Create()
        {
            SetCurrencies();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contracts contract)
        {
            if (!ModelState.IsValid)
            {
                SetCurrencies();
                return View(contract);
            }

            await SaveFile(contract);

            var response = await _httpClient.PostAsJsonAsync(API_URL, contract);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "API error while creating contract.");
                SetCurrencies();
                return View(contract);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var contract = await _httpClient.GetFromJsonAsync<Contracts>($"{API_URL}/{id}");

            if (contract == null)
                return NotFound();

            return View(contract);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var contract = await _httpClient.GetFromJsonAsync<Contracts>($"{API_URL}/{id}");

            if (contract == null)
                return NotFound();

            SetCurrencies();
            return View(contract);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Contracts contract)
        {
            if (id != contract.ContractID)
                return NotFound();

            if (!ModelState.IsValid)
            {
                SetCurrencies();
                return View(contract);
            }

            await SaveFile(contract);

            var response = await _httpClient.PutAsJsonAsync($"{API_URL}/{id}", contract);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "API error while updating contract.");
                SetCurrencies();
                return View(contract);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var contract = await _httpClient.GetFromJsonAsync<Contracts>($"{API_URL}/{id}");

            if (contract == null)
                return NotFound();

            return View(contract);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _httpClient.DeleteAsync($"{API_URL}/{id}");
            return RedirectToAction(nameof(Index));
        }

        private void SetCurrencies()
        {
            ViewBag.Currencies = new SelectList(new List<string>
            {
                "USD", "EUR", "JPY", "ZAR"
            });
        }

        private async Task SaveFile(Contracts contract)
        {
            if (contract.UploadFile == null)
                return;

            string folder = Path.Combine(_environment.WebRootPath, "FileServer", "Contracts");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid() + Path.GetExtension(contract.UploadFile.FileName);

            string filePath = Path.Combine(folder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await contract.UploadFile.CopyToAsync(stream);

            contract.FileName = contract.UploadFile.FileName;
            contract.FilePath = "/FileServer/Contracts/" + fileName;
        }
    }
}