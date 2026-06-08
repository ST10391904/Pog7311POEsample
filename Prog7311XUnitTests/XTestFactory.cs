using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PROG7311POEAPI.DB;
using PROG7311POEAPI.Models;
using PROG7311POEAPI.Services;

namespace Prog7311XUnitTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {

                var dbDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (dbDescriptor != null)
                    services.Remove(dbDescriptor);

                var currency = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ICurrency));

                if (currency != null)
                    services.Remove(currency);

                services.AddScoped<ICurrency, FakeCurrencyService>();

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                SeedData(db);
            });
        }

        private static void SeedData(AppDbContext db)
        {
            if (db.clientDetails.Any())
                return;
                
            //Clients
            var client1 = new ClientDetails
            {
                ClientName = "Client I",
                ClientEmail = "clientI@gmail.com",
                ClientPhoneNumber = "4567896758"
            };

            var client2 = new ClientDetails
            {
                ClientName = "Client II",
                ClientEmail = "clientII@gmail.com",
                ClientPhoneNumber = "1584358900"
            };

            db.clientDetails.AddRange(client1, client2);
            db.SaveChanges();

            var c1Id = client1.ClientID;
            var c2Id = client2.ClientID;

           //Contracts
            var contract1 = new Contracts
            {
                ContractName = "Contract I",
                ClientID = c1Id,
                Currency = "USD",
                Amount = 100,
                AmountInZAR = 1800,
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30)),
                Status = ContractStatus.Active
            };

            var contract2 = new Contracts
            {
                ContractName = "Contract II",
                ClientID = c2Id,
                Currency = "EUR",
                Amount = 200,
                AmountInZAR = 3600,
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(10)),
                Status = ContractStatus.Draft
            };

            db.Contracts.AddRange(contract1, contract2);
            db.SaveChanges();

            // Logistics Manager
            db.LogisticsManagers.Add(new LogisticsManager
            {
                ContractId = contract1.ContractID,
                ContractName = contract1.ContractName,
                ClientName = client1.ClientName,
                Status = ContractStatus.Active,
                Amount = 100,
                Currency = "USD",
                AmountInZAR = 1800,
                StartDate = contract1.StartDate,
                EndDate = contract1.EndDate
            });

            db.SaveChanges();
        }
    }

    public class FakeCurrencyService : ICurrency
    {
        public Task<decimal> ConvertToZAR(string fromCurrency, decimal amount)
        {
            return Task.FromResult(amount * 18);
        }
    }
}