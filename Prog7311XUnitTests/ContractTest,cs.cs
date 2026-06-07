using Xunit;
using Microsoft.AspNetCore.Mvc;
using Prog7311PoeTwo;
using Prog7311PoeTwo.Models;
using Prog7311PoeTwo.Services;
using Prog7311PoeTwo.Controllers;
using Moq;
using System.Linq;
using System.Collections.Generic;

public class ContractTest
{
    [Fact]
    public async Task Index_ReturnsContracts()
    {
        var context = DbContextFactory.Create();

        var client = new ClientDetails
        {
            ClientID = 1,
            ClientName = "YE",
            ClientEmail = "YE@gmail.com",
            ClientPhoneNumber = "9087658970"
        };

        context.clientDetails.Add(client);

        context.Contracts.Add(new Contracts
        {
            ContractID = 1,
            ContractName = "Import",
            ClientID = 1,
            Amount = 100m,
            Currency = "EUR"
        });

        await context.SaveChangesAsync();

        var mockCurrency = new Mock<ICurrency>();

        mockCurrency
            .Setup(x => x.ConvertToZAR(It.IsAny<string>(), It.IsAny<decimal>()))
            .ReturnsAsync(1800m);

        var controller = new ContractsController(context, mockCurrency.Object);

        var result = await controller.Index();

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<List<Contracts>>(view.Model);

        Assert.Single(model);
    }

    [Fact]
    public async Task Create_Post_ConvertsCurrency_AndSaves()
    {
        var context = DbContextFactory.Create();

        context.clientDetails.Add(new ClientDetails
        {
            ClientID = 1,
            ClientName = "YE",
            ClientEmail = "Ye@gmail.com",
            ClientPhoneNumber = "9087658970"
        });

        await context.SaveChangesAsync();

        var mockCurrency = new Mock<ICurrency>();

        mockCurrency
            .Setup(x => x.ConvertToZAR(It.IsAny<string>(), It.IsAny<decimal>()))
            .ReturnsAsync(1800m);

        var controller = new ContractsController(context, mockCurrency.Object);

        var contract = new Contracts
        {
            ContractName = "Import",
            Currency = "EUR",
            Amount = 100m,
            ClientID = 1
        };

        var result = await controller.Create(contract);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);

        Assert.Equal(1800m, context.Contracts.First().AmountInZAR);
    }
}