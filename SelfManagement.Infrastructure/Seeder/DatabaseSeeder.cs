using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SelfManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfManagement.Infrastructure.Seeder
{
    public static class DatabaseSeeder 
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            await RoleSeeder.SeedAsync(roleManager);

        }
    }
}
