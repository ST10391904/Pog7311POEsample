using System.Net;
using Xunit;

namespace Prog7311XUnitTests
{
    public class LogManTests
    {
        private readonly HttpClient _client;

        public LogManTests()
        {
            var factory = new CustomWebApplicationFactory();
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_LogMan_ReturnsOk()
        {
            var res = await _client.GetAsync("/api/logman");

            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }

        [Fact]
        public async Task Get_LogMan_Filter_ReturnsOk()
        {
            var res = await _client.GetAsync("/api/logman?clientName=Seed");

            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }
    }
}