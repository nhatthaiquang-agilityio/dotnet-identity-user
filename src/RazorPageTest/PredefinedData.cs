using IdentityUsers.Models;

namespace RazorPageTest
{
    public class PredefinedData
    {
        public static string Password = "YourPassword01!";
        public static string Email = "testing@gmail.com";

        public static ApplicationUser Profile = new ApplicationUser { Email = Email, UserName = Email };
    }
}
