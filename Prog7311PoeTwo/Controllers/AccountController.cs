using Microsoft.AspNetCore.Mvc;
using Prog7311PoeTwo.Models;
using System.Text;
using System.Text.Json;

namespace Prog7311PoeTwo.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public AccountController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var apiKey = _config["Firebase:ApiKey"];

            var data = new
            {
                email = model.Email,
                password = model.Password,
                returnSecureToken = true
            };

            var content = new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json");

            var response = await _http.PostAsync(
                $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={apiKey}",
                content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Login");

            ModelState.AddModelError("", "Registration failed.");

            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var apiKey = _config["Firebase:ApiKey"];

            var data = new
            {
                email = model.Email,
                password = model.Password,
                returnSecureToken = true
            };

            var content = new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json");

            var response = await _http.PostAsync(
                $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={apiKey}",
                content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            HttpContext.Session.SetString("UserEmail", model.Email);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}