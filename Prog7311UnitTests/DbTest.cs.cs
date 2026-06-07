using System;
using Microsoft.EntityFrameworkCore;
using Prog7311PoeTwo.Models; 

namespace Prog7311UnitTests
{
    public class DbTest
    {
        protected DbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DbContext(options);
        }
    }
}