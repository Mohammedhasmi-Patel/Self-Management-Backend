using Microsoft.AspNetCore.Identity;
using SelfManagement.Domain.Entities;
using SelfManagement.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfManagement.Infrastructure.Seeder
{
    public class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
        {
            foreach (var role in Enum.GetNames<ApplicationUserRole>())
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = role,
                        NormalizedName = role.ToUpperInvariant()
                    });
                }
            }
        }
    }
}
