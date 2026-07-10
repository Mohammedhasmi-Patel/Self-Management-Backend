using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.RepositoryInterface;
using SelfManagement.Application.ServiceInterface;
using SelfManagement.Application.Services;
using SelfManagement.Domain.Entities;
using SelfManagement.Infrastructure.Database;
using SelfManagement.Infrastructure.Repository;
using System.Text.Json;


namespace SelfManagement.API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services,IConfiguration configuration)
        {

            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            #region start controller here
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                })
                .ConfigureApiBehaviorOptions(options =>
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
