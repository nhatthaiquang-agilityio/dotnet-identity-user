using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RazorPageTest
{
    public static class Utilities
    {
        #region snippet1
        public static DbContextOptions<IdentityUsers.Data.AppDbContext> TestDbContextOptions()
        {
            var connectString = $"Server=localhost;Database=test;User Id=sa;Password=Your_password123;";

            // Create a new service provider to create a new in-memory database.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();


            // Create a new options instance using an in-memory database and
            // IServiceProvider that the context should resolve all of its
            // services from.
            var builder = new DbContextOptionsBuilder<IdentityUsers.Data.AppDbContext>()
                .UseSqlServer(connectString)
                .UseInternalServiceProvider(serviceProvider);
            return builder.Options;
        }
        #endregion
    }
}
