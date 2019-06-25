using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityUsers.Data;
using IdentityUsers.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RazorPageTest
{
    public static class Utilities
    {
        #region snippet1
        public static DbContextOptions<AppDbContext> TestDbContextOptions()
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Test_db");
            return builder.Options;
        }
        #endregion

        public static async Task SeedDataTest(AppDbContext db)
        {
            IPasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();
            var validator = new UserValidator<ApplicationUser>();
            var validators = new List<UserValidator<ApplicationUser>> { validator };
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(db), null, hasher, validators, null, null, null, null, null);

            // Add all the predefined profiles using the predefined password
            try
            {
                var result = await userManager.CreateAsync(PredefinedData.Profile, PredefinedData.Password);
                var user = await userManager.FindByEmailAsync(PredefinedData.Email);
                Console.WriteLine(user.PasswordHash);
                if (result.Succeeded)
                {
                    Console.WriteLine("save user done");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Exception");
                Console.WriteLine(e.Message);
            }
                
            
            db.SaveChanges();
        }
    }
}
