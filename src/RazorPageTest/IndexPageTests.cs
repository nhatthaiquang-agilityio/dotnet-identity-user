using System.Threading.Tasks;
using IdentityUsers.Hubs;
using IdentityUsers.Pages.Account;
using IdentityUsers.Service;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;

namespace RazorPageTest
{
    public class IndexPageTests
    {

        [Fact]
        public void OnGet_HomePageModel()
        {
            var pageModel = new IdentityUsers.Pages.IndexModel();
            pageModel.OnGet();
        }

        [Fact]
        public async Task OnPostAsync_ReturnAsPageResult()
        {
            var hub = new Mock<IHubContext<NotificationUserHub>>();
            var userManager = new Mock<IUserConnectionManager>();
            var pageModel = new MessageModel(hub.Object, userManager.Object);

            // Act
            var result = await pageModel.OnPostAsync("Hello");

            // Assert
            Assert.IsType<PageResult>(result);
        }
    }
}