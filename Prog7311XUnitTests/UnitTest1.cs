using Xunit;
using Microsoft.AspNetCore.Mvc;
using Prog7311PoeTwo.Controllers;
using Prog7311PoeTwo;
using Prog7311PoeTwo.Models;
using Prog7311PoeTwo.Services;
using Microsoft.EntityFrameworkCore;

public static class DbContextFactory
{
    public static AppDbContext Create()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}