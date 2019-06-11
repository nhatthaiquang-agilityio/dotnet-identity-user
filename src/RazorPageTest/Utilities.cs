using Microsoft.EntityFrameworkCore;

namespace RazorPageTest
{
    public static class Utilities
    {
        #region snippet1
        public static DbContextOptions<IdentityUsers.Data.AppDbContext> TestDbContextOptions()
        {
            var builder = new DbContextOptionsBuilder<IdentityUsers.Data.AppDbContext>()
                .UseInMemoryDatabase("Test_db");
            return builder.Options;
        }
        #endregion
    }
}
