using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;

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
                // Add a database context (ApplicationDbContext) using an in-memory 
                // database for testing.
                services.AddDbContext<IdentityUsers.Data.AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("test_db");
                });
            });

        }

    }

}
