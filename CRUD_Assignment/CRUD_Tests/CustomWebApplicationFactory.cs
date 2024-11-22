using Microsoft.AspNetCore.Builder;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Entities;
using Microsoft.Extensions.DependencyInjection;

namespace CRUD_Tests
{
    // Custom factory for configuring the test web host environment
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        // Override to customize the web host for integration tests
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Call the base configuration for the web host
            base.ConfigureWebHost(builder);

            // Set the environment to "Test" for isolation
            builder.UseEnvironment("Test");

            // Customize the application's service collection
            builder.ConfigureServices(services =>
            {
                // Find the existing registration for ApplicationDbContext
                var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                // Remove the existing registration if found
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add an in-memory database for ApplicationDbContext
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemory Database");
                });
            });
        }
    }

}
