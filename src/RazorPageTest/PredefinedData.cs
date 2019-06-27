using IdentityUsers.Models;

namespace RazorPageTest
{
    public static class PredefinedData
    {
        public static string Password = "YourPassword01!";
        public static string Email = "testing@gmail.com";
        public static string Register_Email = "register@gmail.com";
        public static string Login_Email = "login@gmail.com";
        public static ApplicationUser Profile = new ApplicationUser { Email = Email, UserName = Email };
    }
}
