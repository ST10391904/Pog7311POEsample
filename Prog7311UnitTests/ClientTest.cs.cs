using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Prog7311PoeTwo.Controllers;
using Prog7311PoeTwo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

[TestFixture]
public class ClientTests
{
    private AppDbContext _context;
    private ClientController _controller;

    [SetUp]
    public void Setup()
    {
        _context = GetDbContext();
        _controller = new ClientController(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        _controller = null;
    }

    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Test]
    public async Task Index_ReturnsViewWithClients()
    {
        _context.clientDetails.Add(new ClientDetails
        {
            ClientName = "Test Client"
        });

        await _context.SaveChangesAsync();

        var result = await _controller.Index() as ViewResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Model, Is.Not.Null);
    }

    [Test]
    public async Task Create_Post_ValidModel_Redirects()
    {
        var client = new ClientDetails
        {
            ClientName = "New Client"
        };

        var result = await _controller.Create(client);

        Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
    }

    [Test]
    public async Task Details_InvalidId_ReturnsNotFound()
    {
        var result = await _controller.Details(999);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }
}