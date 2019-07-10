using System.Threading.Tasks;
using IdentityUsers.Data;
using Microsoft.AspNetCore.Identity;

namespace IdentityUsers.Models
{
    public static class DbInitializer
    {
        public static async Task Initialize(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            context.Database.EnsureCreated();
            await CreateDefaultUser(userManager);
        }

        private static async Task CreateDefaultUser(UserManager<IdentityUser> userManager)
        {
            var user = new IdentityUser { Email = "testing@gmail.com", UserName = "testing@gmail.com" };
            await userManager.CreateAsync(user, "testing@123456");
        }
    }
}
