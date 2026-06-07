using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Prog7311PoeTwo.Models;
using Prog7311PoeTwo.Services;

namespace Prog7311PoeTwo.Controllers
{
    public class ContractsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICurrency _currencyService;
        private readonly IWebHostEnvironment _environment;

        public ContractsController(
            AppDbContext context,
            ICurrency currencyService,
            IWebHostEnvironment environment)
        {
            _context = context;
            _currencyService = currencyService;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var contracts = await _context.Contracts
                .Include(c => c.ClientDetails)
                .ToListAsync();

            return View(contracts);
        }

        public IActionResult Create()
        {
            ViewBag.Clients = new SelectList(_context.clientDetails, "ClientID", "ClientName");

            ViewBag.Currencies = new SelectList(new List<string>
            {
                "USD", "EUR", "Yen", "ZAR"
            });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contracts contract)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        contract.AmountInZAR =
                            await _currencyService.ConvertToZAR(
                                contract.Currency,
                                contract.Amount);
                    }
                    catch
                    {
                        contract.AmountInZAR = 0;
                    }

                   //File Upload
                    if (contract.UploadFile != null)
                    {
                        string folder = Path.Combine(
                            _environment.WebRootPath,
                            "FileServer",
                            "Contracts");

                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }

                        string fileName = Guid.NewGuid().ToString() +
                                          Path.GetExtension(contract.UploadFile.FileName);

                        string filePath = Path.Combine(folder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await contract.UploadFile.CopyToAsync(stream);
                        }

                        contract.FileName = contract.UploadFile.FileName;
                        contract.FilePath = "/FileServer/Contracts/" + fileName;
                    }

                    _context.Add(contract);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Failed to create contract.");
            }

            ViewBag.Clients = new SelectList(
                _context.clientDetails,
                "ClientID",
                "ClientName",
                contract.ClientID);

            ViewBag.Currencies = new SelectList(new List<string>
            {
                "USD", "EUR", "Yen", "ZAR"
            });

            return View(contract);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var contract = await _context.Contracts
                .Include(c => c.ClientDetails)
                .FirstOrDefaultAsync(c => c.ContractID == id);

            if (contract == null)
                return NotFound();

            return View(contract);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null)
                return NotFound();

            ViewBag.Clients = new SelectList(
                _context.clientDetails,
                "ClientID",
                "ClientName",
                contract.ClientID);

            ViewBag.Currencies = new SelectList(new List<string>
            {
                "USD",
                "EUR",
                "JPY",
                "ZAR"
            });

            return View(contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contracts contract)
        {
            if (id != contract.ContractID)
                return NotFound();

            if (ModelState.IsValid)
            {
                contract.AmountInZAR =
                    await _currencyService.ConvertToZAR(
                        contract.Currency,
                        contract.Amount);

                if (contract.UploadFile != null)
                {
                    string folder = Path.Combine(
                        _environment.WebRootPath,
                        "FileServer",
                        "Contracts");

                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    string fileName = Guid.NewGuid().ToString() +
                                      Path.GetExtension(contract.UploadFile.FileName);

                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await contract.UploadFile.CopyToAsync(stream);
                    }

                    contract.FileName = contract.UploadFile.FileName;
                    contract.FilePath = "/FileServer/Contracts/" + fileName;
                }

                _context.Update(contract);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clients = new SelectList(
                _context.clientDetails,
                "ClientID",
                "ClientName",
                contract.ClientID);

            ViewBag.Currencies = new SelectList(new List<string>
            {
                "USD",
                "EUR",
                "JPY",
                "ZAR"
            });

            return View(contract);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var contract = await _context.Contracts
                .Include(c => c.ClientDetails)
                .FirstOrDefaultAsync(m => m.ContractID == id);

            if (contract == null)
                return NotFound();

            return View(contract);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract != null)
            {
                _context.Contracts.Remove(contract);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}