using Microsoft.AspNetCore.Mvc;
using SelfManagement.Application.ServiceInterface;
using SelfManagement.Application.Services;
using SelfManagement.Application.DTO.Common;
using SelfManagement.Domain.Entities;
using SelfManagement.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using SelfManagement.Application.RepositoryInterface;
using SelfManagement.Infrastructure.Repository;


namespace SelfManagement.API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services,IConfiguration configuration)
        {

            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            #region start controller here
            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var firstErrorMessage = context.ModelState
                                            .Where(e => e.Value?.Errors.Count > 0)
                                            .Select(e => e.Value?.Errors.First().ErrorMessage)
                                            .FirstOrDefault() ?? "A validation error occurred.";
                    var failureResponse = ApiResponse<object>.FailureResponse(firstErrorMessage);

                    return new BadRequestObjectResult(failureResponse);
                };
            });
            #endregion End here controller

            services.AddIdentity<ApplicationUser,ApplicationRole>()
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IOtpRepository,OtpRepository>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
    }
}
