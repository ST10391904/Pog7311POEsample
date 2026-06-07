using Xunit;
using Microsoft.AspNetCore.Mvc;
using Prog7311PoeTwo;
using Prog7311PoeTwo.Models;
using Prog7311PoeTwo.Services;
using Prog7311PoeTwo.Controllers;
using Moq;

public class LogManTest
{
    [Fact]
    public async Task Index_ReturnsMappedContracts()
    {
        var context = DbContextFactory.Create();

        context.clientDetails.Add(new ClientDetails
        {
            ClientID = 1,
            ClientName = "Kanye West",
            ClientEmail = "kanye@gmail.com",
            ClientPhoneNumber = "0649015674"
        });

        context.Contracts.Add(new Contracts
        {
            ContractID = 1,
            ContractName = "Export",
            ClientID = 1,
            Currency = "USD",
            Amount = 100m
        });

        await context.SaveChangesAsync();

        var mockCurrency = new Mock<ICurrency>();

        mockCurrency
            .Setup(x => x.ConvertToZAR(It.IsAny<string>(), It.IsAny<decimal>()))
            .ReturnsAsync(1800m);

        var controller = new LogManController(context, mockCurrency.Object);

        var result = await controller.Index(null, null, null, null);

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<LogisticsManager[]>(view.Model);

        Assert.Single(model);
        Assert.Equal("Export", model[0].ContractName);
    }

    [Fact]
    public void RequestSLA_ReturnsContent()
    {
        var context = DbContextFactory.Create();
        var mockCurrency = new Mock<ICurrency>();

        var controller = new LogManController(context, mockCurrency.Object);

        var result = controller.RequestSLA(5);

        var content = Assert.IsType<ContentResult>(result);
        Assert.Contains("5", content.Content);
    }
}