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
public async Task<IActionResult> Create([FromBody] ContractDTO dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    var contract = new Contracts
    {
        ContractName = dto.ContractName,
        ClientID = dto.ClientID,
        Currency = dto.Currency,
        Amount = dto.Amount,
        StartDate = dto.StartDate,
        EndDate = dto.EndDate,
        Status = dto.Status,
        FileName = dto.FileName,
        FilePath = dto.FilePath
    };

    contract.AmountInZAR = await Convert(contract);

    _context.Contracts.Add(contract);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetById), new { id = contract.ContractID }, contract);
}

[HttpPut("{id}")]
public async Task<IActionResult> Update(int id, [FromBody] ContractDTO dto)
{
    var contract = await _context.Contracts.FindAsync(id);

    if (contract == null)
        return NotFound();

    contract.ContractName = dto.ContractName;
    contract.ClientID = dto.ClientID;
    contract.Currency = dto.Currency;
    contract.Amount = dto.Amount;
    contract.StartDate = dto.StartDate;
    contract.EndDate = dto.EndDate;
    contract.Status = dto.Status;
    contract.FileName = dto.FileName;
    contract.FilePath = dto.FilePath;

    contract.AmountInZAR = await Convert(contract);

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
            
                return await _currencyService.ConvertToZAR(
                    contract.Currency,
                    contract.Amount
                );
            
        
    }

    public class ContractDTO
    {
    public string? ContractName { get; set; }
    public int ClientID { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public ContractStatus Status { get; set; }

    public string? FileName { get; set; }
    public string? FilePath { get; set; }
    }
}
}