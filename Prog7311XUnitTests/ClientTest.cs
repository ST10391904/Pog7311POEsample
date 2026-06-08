using System.Net;
using System.Net.Http.Json;
using PROG7311POEAPI.Models;
using Xunit;

namespace Prog7311XUnitTests
{
    public class ClientIntegrationTests
    {
        private readonly HttpClient _client;

        public ClientIntegrationTests()
        {
            var factory = new CustomWebApplicationFactory();
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Clients_ReturnsOk()
        {
            var res = await _client.GetAsync("/api/clients");

            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }

        [Fact]
        public async Task Create_Client_ReturnsSuccess()
        {
            var client = new ClientDetails
            {
                ClientName = "Test Client",
                ClientEmail = "TC@gmail.com",
                ClientPhoneNumber = "123 456 7890"
            };

            var res = await _client.PostAsJsonAsync("/api/clients", client);

            Assert.True(res.StatusCode == HttpStatusCode.Created ||
                        res.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async Task Client_Full_CRUD_Flow()
        {
            var client = new ClientDetails
            {
                ClientName = "Client One",
                ClientEmail = "CO@gmail.com",
                ClientPhoneNumber = "0986781234"
            };

            var create = await _client.PostAsJsonAsync("/api/clients", client);
            var created = await create.Content.ReadFromJsonAsync<ClientDetails>();

            var get = await _client.GetAsync($"/api/clients/{created!.ClientID}");
            Assert.Equal(HttpStatusCode.OK, get.StatusCode);

            created.ClientName = "Updated Name";
            var update = await _client.PutAsJsonAsync($"/api/clients/{created.ClientID}", created);
            Assert.Equal(HttpStatusCode.NoContent, update.StatusCode);

            var delete = await _client.DeleteAsync($"/api/clients/{created.ClientID}");
            Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);
        }
    }
}