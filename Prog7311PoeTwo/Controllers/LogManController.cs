using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog7311PoeTwo.Models;
using Prog7311PoeTwo.Services;

namespace Prog7311PoeTwo.Controllers
{
    public class LogManController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICurrency _currencyService;

        public LogManController(AppDbContext context, ICurrency currencyService)
        {
            _context = context;
            _currencyService = currencyService;
        }

        public async Task<IActionResult> Index(
            string? clientName,
            ContractStatus? status,
            DateOnly? startDate,
            DateOnly? endDate)
        {
            var query = _context.Contracts
                .Include(c => c.ClientDetails)
                .AsQueryable();

            query = query
                .Where(c => string.IsNullOrWhiteSpace(clientName)
                    || c.ClientDetails.ClientName.Contains(clientName))

                .Where(c => !status.HasValue
                    || c.Status == status.Value)

                .Where(c => !startDate.HasValue
                    || c.StartDate >= startDate.Value)

                .Where(c => !endDate.HasValue
                    || c.EndDate <= endDate.Value);

            var contracts = await query.ToListAsync();

            var result = await Task.WhenAll(contracts.Select(async contract =>
            {
                var zar = await _currencyService.ConvertToZAR(
                    contract.Currency,
                    contract.Amount
                );

                return new LogisticsManager
                {
                    ContractId = contract.ContractID,
                    ContractName = contract.ContractName,
                    ClientName = contract.ClientDetails?.ClientName,
                    Status = contract.Status,
                    Amount = contract.Amount,
                    Currency = contract.Currency,
                    AmountInZAR = zar,
                    StartDate = contract.StartDate,
                    EndDate = contract.EndDate
                };
            }));

            ViewBag.ClientName = clientName;
            ViewBag.Status = status;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View(result);
        }

        public IActionResult RequestSLA(int id)
        {
            return Content($"SLA requested for contract {id}");
        }
    }
}