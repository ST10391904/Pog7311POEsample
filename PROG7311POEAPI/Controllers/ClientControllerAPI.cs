using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311POEAPI.Models;
using PROG7311POEAPI.DB;

namespace PROG7311POEAPI.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientControllerApi : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientControllerApi(AppDbContext context)
        {
            _context = context;
        }

        // Get clients
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.clientDetails.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _context.clientDetails.FindAsync(id);

            if (client == null)
                return NotFound();

            return Ok(client);
        }

        //Post Clients
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClientDetails client)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.clientDetails.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = client.ClientID }, client);
        }

        //Update Clients
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClientDetails client)
        {
            if (id != client.ClientID)
                return BadRequest();

            _context.Entry(client).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Delete Clients
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _context.clientDetails.FindAsync(id);

            if (client == null)
                return NotFound();

            _context.clientDetails.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}