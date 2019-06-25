using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using IdentityUsers.Data;


namespace RazorPageTest
{
    public class CustomWebAppFactory<TStartup> : WebApplicationFactory<IdentityUsers.Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.2
            // Add database name for testing
            builder.ConfigureServices(services =>
            {
                // A database context(ApplicationDbContext) using an in-memory
                // database for testing.
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("test_db");
                });

                // Build the service provider.
                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = services.BuildServiceProvider().CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<AppDbContext>();

                    // Ensure the database is created.
                    db.Database.EnsureCreated();

                    // Seed the database with test data.
                    Utilities.SeedDataTest(db).Wait();
                }

            });

        }
    }

}
