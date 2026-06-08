using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Prog7311PoeTwo.Models;

namespace Prog7311PoeTwo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("UserEmail") == null)
    {
        return RedirectToAction("Register", "Account");
    }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

}
