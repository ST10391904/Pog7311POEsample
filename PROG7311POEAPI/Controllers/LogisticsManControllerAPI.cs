using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311POEAPI.Models;
using PROG7311POEAPI.Services;
using PROG7311POEAPI.DB;

namespace PROG7311POEAPI.Controllers
{
    [ApiController]
    [Route("api/logman")]
    public class LogManController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICurrency _currencyService;

        public LogManController(AppDbContext context, ICurrency currencyService)
        {
            _context = context;
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFiltered(
            string? clientName,
            ContractStatus? status,
            DateOnly? startDate,
            DateOnly? endDate)
        {
            var query = _context.Contracts
                .Include(c => c.ClientDetails)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(clientName))
                query = query.Where(c => c.ClientDetails.ClientName.Contains(clientName));

            if (status.HasValue)
                query = query.Where(c => c.Status == status.Value);

            if (startDate.HasValue)
                query = query.Where(c => c.StartDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(c => c.EndDate <= endDate.Value);

            var contracts = await query.ToListAsync();

            var result = new List<LogisticsManager>();

            foreach (var contract in contracts)
            {
                decimal zar = 0;

                try
                {
                    zar = await _currencyService.ConvertToZAR(
                        contract.Currency,
                        contract.Amount);
                }
                catch
                {
                    zar = 0;
                }

                result.Add(new LogisticsManager
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
                });
            }

            return Ok(result);
        }

        [HttpGet("sla/{id}")]
        public IActionResult RequestSLA(int id)
        {
            return Ok($"SLA requested for contract {id}");
        }
    }
}