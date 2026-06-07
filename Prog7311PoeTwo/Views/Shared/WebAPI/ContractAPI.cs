using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog7311PoeTwo.Models;
using Prog7311PoeTwo.Services;

namespace Prog7311PoeTwo.Controllers.Api
{
    [ApiController]
    [Route("api/contracts")]
    public class ContractsApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICurrency _currencyService;

        public ContractsApiController(AppDbContext context, ICurrency currencyService)
        {
            _context = context;
            _currencyService = currencyService;
        }

        // get contracts
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contracts = await _context.Contracts
                .Include(c => c.ClientDetails)
                .ToListAsync();

            return Ok(contracts);
        }

        // get contracts
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.ClientDetails)
                .FirstOrDefaultAsync(c => c.ContractID == id);

            if (contract == null)
                return NotFound();

            return Ok(contract);
        }

        // post contracts
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Contracts contract)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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

            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = contract.ContractID },
                contract
            );
        }

        // delete contracts
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null)
                return NotFound();

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}