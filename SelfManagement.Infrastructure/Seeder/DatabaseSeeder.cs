using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SelfManagement.Domain.Entities;
using SelfManagement.Infrastructure.Database;

namespace SelfManagement.Infrastructure.Seeder
{
    public static class DatabaseSeeder 
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            // get application db context
            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //await RoleSeeder.SeedAsync(roleManager);
            //await CountryStateCitySeeder.SeedAsync(applicationDbContext);

        }
    }
}
