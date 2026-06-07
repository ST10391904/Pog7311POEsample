using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog7311PoeTwo.Models;

namespace Prog7311PoeTwo.Controllers.Api
{
    [ApiController]
    [Route("api/contracts")]
    public class ContractsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContractsApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        

    }
}