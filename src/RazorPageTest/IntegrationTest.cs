using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace RazorPageTest
{
    public class IntegrationTest : IClassFixture<CustomWebAppFactory<IdentityUsers.Startup>>
    {
        private readonly HttpClient _client;

        public IntegrationTest(CustomWebAppFactory<IdentityUsers.Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Index_Get_Index()
        {
            // Act
            var response = await _client.GetAsync("/");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
