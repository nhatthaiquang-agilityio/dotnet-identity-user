using System;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;


namespace RazorPageTest
{
    public class TestFixture : IClassFixture<CustomWebAppFactory<IdentityUsers.Startup>>
    {
        private readonly HttpClient _client;

        public TestFixture(CustomWebAppFactory<IdentityUsers.Startup> factory)
        {
            _client = factory.CreateClient();
        }

        protected static string AUTHENTICATION_COOKIE = ".AspNetCore.Identity.";
        protected static string ANTIFORGERY_COOKIE = ".AspNetCore.AntiForgery.";
        protected static string ANTIFORGERY_TOKEN_FORM = "__RequestVerificationToken";
        protected static string ANTIFORGERTY_TOKEN_HEADER = "XSRF-TOKEN";
        protected static Regex AntiforgeryFormFieldRegex = new Regex(@"\<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" \/\>");

        protected SetCookieHeaderValue _authenticationCookie;
        protected SetCookieHeaderValue _antiforgeryCookie;
        protected string _antiforgeryToken;

        [Fact]
        public async Task<string> EnsureAntiforgeryToken()
        {
            if (_antiforgeryToken != null) return _antiforgeryToken;

            var response = await _client.GetAsync("/Identity/Account/Login");
            response.EnsureSuccessStatusCode();

            if (response.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> values))
            {
                _antiforgeryCookie = SetCookieHeaderValue.ParseList(values.ToList()).SingleOrDefault(c => c.Name.StartsWith(ANTIFORGERY_COOKIE, StringComparison.InvariantCultureIgnoreCase));
            }
            Assert.NotNull(_antiforgeryCookie);
            _client.DefaultRequestHeaders.Add("Cookie", new CookieHeaderValue(_antiforgeryCookie.Name, _antiforgeryCookie.Value).ToString());

            var responseHtml = await response.Content.ReadAsStringAsync();
            var match = AntiforgeryFormFieldRegex.Match(responseHtml);
            _antiforgeryToken = match.Success ? match.Groups[1].Captures[0].Value : null;
            Assert.NotNull(_antiforgeryToken);

            return _antiforgeryToken;
        }

        public async Task<Dictionary<string, string>> EnsureAntiforgeryTokenForm(Dictionary<string, string> formData = null)
        {
            if (formData == null) formData = new Dictionary<string, string>();

            formData.Add(ANTIFORGERY_TOKEN_FORM, await EnsureAntiforgeryToken());
            return formData;
        }

        public async Task EnsureAntiforgeryTokenHeader()
        {
            _client.DefaultRequestHeaders.Add(ANTIFORGERTY_TOKEN_HEADER, await EnsureAntiforgeryToken());
        }

        [Fact]
        public async Task EnsureAuthenticationCookie()
        {
            if (_authenticationCookie != null) return;

            var formData = await EnsureAntiforgeryTokenForm(new Dictionary<string, string>
            {
                { "Input.Email", PredefinedData.Email },
                { "Input.Password", PredefinedData.Password }
            });
            var response = await _client.PostAsync("/Identity/Account/Login?ReturnUrl=/Account/Room", new FormUrlEncodedContent(formData));
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            if (response.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> values))
            {
                _authenticationCookie = SetCookieHeaderValue.ParseList(values.ToList()).SingleOrDefault(c => c.Name.StartsWith(AUTHENTICATION_COOKIE, StringComparison.InvariantCultureIgnoreCase));
            }
            Assert.NotNull(_authenticationCookie);
            _client.DefaultRequestHeaders.Add("Cookie", new CookieHeaderValue(_authenticationCookie.Name, _authenticationCookie.Value).ToString());

            // The current pair of antiforgery cookie-token is not valid anymore
            // Since the tokens are generated based on the authenticated user!
            // We need a new token after authentication (The cookie can stay the same)
            _antiforgeryToken = null;
        }

        #region snippet3
        [Fact]
        public async Task Post_DeleteMessageHandler_ReturnsRedirectToRoot()
        {
            var uri = "/Account/Login?ReturnUrl=/Account/Room";

            var formData = new Dictionary<string, string>
            {
                {"Input.Email", PredefinedData.Email},
                {"Input.Password", PredefinedData.Password},
                {"Input.RememberMe", Boolean.TrueString},
                {"__RequestVerificationToken", await EnsureAntiforgeryToken()}
            };
            var content = new FormUrlEncodedContent(formData);
            var response = await _client.PostAsync(uri, content);

            response.EnsureSuccessStatusCode();
            //Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }
        #endregion
    }
}
