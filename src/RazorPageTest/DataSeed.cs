using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RazorPageTest
{
    public class DataSeed
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentityUsers.Data.AppDbContext _context;

        public DataSeed(IdentityUsers.Data.AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //public async Task Seed()
        //{
        //    // Add all the predefined profiles using the predefined password
        //    foreach (var profile in PredefinedData.Profiles)
        //    {
        //        await _userManager.CreateAsync(profile, PredefinedData.Password);
                
        //    }

        //    _context.SaveChanges();
        //}
    }
}
