using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog7311PoeTwo.Models;

namespace Prog7311PoeTwo.Controllers
{
    public class ClientController : Controller
    {
        private readonly AppDbContext _context;

        public ClientController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.clientDetails.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientDetails clientDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clientDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(clientDetails);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var client = await _context.clientDetails
                .FirstOrDefaultAsync(c => c.ClientID == id);

            if (client == null) return NotFound();

            return View(client);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var client = await _context.clientDetails.FindAsync(id);
            if (client == null) return NotFound();

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClientDetails clientDetails)
        {
            if (id != clientDetails.ClientID) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(clientDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(clientDetails);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var client = await _context.clientDetails
                .FirstOrDefaultAsync(c => c.ClientID == id);

            if (client == null) return NotFound();

            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.clientDetails.FindAsync(id);

            if (client != null)
            {
                _context.clientDetails.Remove(client);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}