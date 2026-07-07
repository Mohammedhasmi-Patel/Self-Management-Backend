using Microsoft.Extensions.Options;
using SelfManagement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;


namespace SelfManagement.API.Extensions
{
    public static class DatabaseExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseNpgsql(
                 configuration.GetConnectionString("DefaultConnection")!));

            return services;
        }
    }
}
