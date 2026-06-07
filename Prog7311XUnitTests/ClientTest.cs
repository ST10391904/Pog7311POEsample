using Xunit;
using Microsoft.AspNetCore.Mvc;
using Prog7311PoeTwo.Controllers;
using Prog7311PoeTwo;
using Prog7311PoeTwo.Models;
using Moq;
public class ClientTest
{
    [Fact]
    public async Task Index_ReturnsView_WithClients()
    {
        var context = DbContextFactory.Create();

        context.clientDetails.Add(new ClientDetails
        {
            ClientID = 1,
            ClientName = "Scott Mescudi"
        });
        await context.SaveChangesAsync();

        var controller = new ClientController(context);

        var result = await controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<List<ClientDetails>>(viewResult.Model);

        Assert.Single(model);
    }

    [Fact]
    public async Task Create_Post_AddsClient_Redirects()
    {
        var context = DbContextFactory.Create();
        var controller = new ClientController(context);

        var client = new ClientDetails
        {
            ClientName = "Jaques Webster"
        };

        var result = await controller.Create(client);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);

        Assert.Single(context.clientDetails);
    }
}