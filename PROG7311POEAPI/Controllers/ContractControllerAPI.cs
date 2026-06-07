using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311POEAPI.Models;
using PROG7311POEAPI.Services;
using PROG7311POEAPI.DB;

namespace PROG7311POEAPI
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contracts = await _context.Contracts
                .Include(c => c.ClientDetails)
                .ToListAsync();

            return Ok(contracts);
        }

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

        [HttpPost]
        public async Task<IActionResult> Create(Contracts contract)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            contract.AmountInZAR = await Convert(contract);

            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = contract.ContractID }, contract);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Contracts contract)
        {
            if (id != contract.ContractID)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            contract.AmountInZAR = await Convert(contract);

            _context.Entry(contract).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

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

        private async Task<decimal> Convert(Contracts contract)
        {
            try
            {
                return await _currencyService.ConvertToZAR(contract.Currency, contract.Amount);
            }
            catch
            {
                return 0;
            }
        }
    }
}