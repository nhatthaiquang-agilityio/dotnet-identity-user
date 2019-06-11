using IdentityUsers.Pages;
using Xunit;

namespace RazorPageTest
{
    public class IndexPageTests
    {
       
        [Fact]
        public void OnGet_HomePageModel()
        {
            var pageModel = new IndexModel();
            pageModel.OnGet();
        }
    }
}