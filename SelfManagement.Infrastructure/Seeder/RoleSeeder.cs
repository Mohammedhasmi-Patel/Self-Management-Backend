using Microsoft.AspNetCore.Identity;
using SelfManagement.Domain.Entities;
using SelfManagement.Domain.Enum;


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
