using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace RazorPageTest
{
    public static class AntiForgeryHelper
    {
        public static string ExtractAntiForgeryToken(string htmlResponseText)
        {
            if (htmlResponseText == null) throw new ArgumentNullException(nameof(htmlResponseText));

            Match match = Regex.Match(htmlResponseText, @"\<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" \/\>");
            return match.Success ? match.Groups[1].Captures[0].Value : null;
        }

        public static async Task<string> ExtractAntiForgeryToken(HttpResponseMessage response)
        {
            string responseAsString = await response.Content.ReadAsStringAsync();
            return await Task.FromResult(ExtractAntiForgeryToken(responseAsString));
        }
    }

}
