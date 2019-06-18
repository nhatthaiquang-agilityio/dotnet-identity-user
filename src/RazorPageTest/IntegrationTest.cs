using System;
using System.Collections.Generic;
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

        [Fact]
        public async Task Index_Get_Room()
        {
            // Act
            var response = await _client.GetAsync("/Account/Room");

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal("/Identity/Account/Login", response.RequestMessage.RequestUri.LocalPath);
        }

        [Fact]
        public async Task Index_Get_Message()
        {
            // Act
            var response = await _client.GetAsync("/Account/Message");

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal("/Identity/Account/Login", response.RequestMessage.RequestUri.LocalPath);
        }

        [Fact]
        public async Task Index_Get_Contact()
        {
            // Act
            var response = await _client.GetAsync("/Contact");

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal("/Contact", response.RequestMessage.RequestUri.LocalPath);
            Assert.Equal("/Contact", response.RequestMessage.RequestUri.AbsolutePath);
        }

        [Fact]
        public async Task Index_Get_Register()
        {
            // Act
            var response = await _client.GetAsync("/Identity/Account/Register");

            // Assert
            response.EnsureSuccessStatusCode();

        }

        [Fact]
        public async Task Index_Post_Register()
        {
            var verificationToken = GetVerificationToken(_client, "https://localhost/Identity/Account/Register");

            var formData = new Dictionary<string, string>
             {
                {"Email", "testing@gmail.com"},
                {"Password", "testing@123"},
                {"__RequestVerificationToken", verificationToken} 
             };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Register")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            var response = await _client.SendAsync(postRequest);

            response.EnsureSuccessStatusCode();
            //var responseString = await response.Content.ReadAsStringAsync();
        }

        private string GetVerificationToken(HttpClient client, string url)
        {
            HttpResponseMessage response = client.GetAsync(url).Result;
            var verificationToken = response.Content.ReadAsStringAsync().Result;

            if (!string.IsNullOrEmpty(verificationToken) && verificationToken.Length > 0)
            {
                verificationToken = verificationToken.Substring(
                    verificationToken.IndexOf("__RequestVerificationToken", StringComparison.Ordinal));
                verificationToken = verificationToken.Substring(
                    verificationToken.IndexOf("value=\"", StringComparison.Ordinal) + 7);
                verificationToken = verificationToken.Substring(
                    0, verificationToken.IndexOf("\"", StringComparison.Ordinal));
            }

            return verificationToken;
        }
    }
}
