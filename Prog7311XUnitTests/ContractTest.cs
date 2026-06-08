using System.Net;
using System.Net.Http.Json;
using Xunit;
using static PROG7311POEAPI.ContractsApiController;

namespace Prog7311XUnitTests
{
    public class ContractIntegrationTests
    {
        private readonly HttpClient _client;

        public ContractIntegrationTests()
        {
            var factory = new CustomWebApplicationFactory();
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Contract_CRUD_Flow()
        {
            var dto = new ContractDTO
            {
                ContractName = "Contract One",
                ClientID = 1,
                Currency = "USD",
                Amount = 200,
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(10)),
                Status = 0 //Draft
            };

            var create = await _client.PostAsJsonAsync("/api/contracts", dto);

            Assert.True(create.StatusCode == HttpStatusCode.Created ||
                        create.StatusCode == HttpStatusCode.OK);

            var contract = await create.Content.ReadFromJsonAsync<dynamic>();

            var get = await _client.GetAsync("/api/contracts/1");
            Assert.Equal(HttpStatusCode.OK, get.StatusCode);

            dto.ContractName = "Updated Contract";
            var update = await _client.PutAsJsonAsync("/api/contracts/1", dto);
            Assert.Equal(HttpStatusCode.NoContent, update.StatusCode);

            var delete = await _client.DeleteAsync("/api/contracts/1");
            Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);
        }
    }
}