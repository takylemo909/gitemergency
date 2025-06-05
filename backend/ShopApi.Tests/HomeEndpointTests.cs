using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ShopApi.Tests
{
    public class HomeEndpointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public HomeEndpointTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetRoot_ReturnsWelcomeMessage()
        {
            using var client = _factory.CreateClient();
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("admin:password"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            var response = await client.GetAsync("/");
            var body = await response.Content.ReadAsStringAsync();
            Assert.Equal("Welcome to the Simple Shop API!", body);
        }
    }
}
