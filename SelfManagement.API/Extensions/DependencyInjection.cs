using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.RepositoryInterface;
using SelfManagement.Application.RepositoryInterface.Common;
using SelfManagement.Application.RepositoryInterface.Company;
using SelfManagement.Application.RepositoryInterface.Locations;
using SelfManagement.Application.ServiceInterface;
using SelfManagement.Application.ServiceInterface.Auth;
using SelfManagement.Application.ServiceInterface.Company;
using SelfManagement.Application.ServiceInterface.FileStorage;
using SelfManagement.Application.ServiceInterface.Location;
using SelfManagement.Application.Services.Auth;
using SelfManagement.Application.Services.Companies;
using SelfManagement.Application.Services.FileStorage;
using SelfManagement.Application.Services.Location;
using SelfManagement.Domain.Entities;
using SelfManagement.Infrastructure.Common;
using SelfManagement.Infrastructure.Database;
using SelfManagement.Infrastructure.Repository;
using SelfManagement.Infrastructure.Repository.Location;
using System.Text;
using System.Text.Json;


namespace SelfManagement.API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services,IConfiguration configuration)
        {
            JwtConfiguration jwtConfiguration = configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>()!;
            
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.Configure<JwtConfiguration>(configuration.GetSection("JwtConfiguration"));
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

            services.AddScoped<ICompaniesRepository, CompaniesRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IOtpRepository,OtpRepository>();
            services.AddScoped<IJwtService,JwtService>();
            services.AddScoped<IFileService,FileService>();
            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<ICitiesRepository, CityRepository>();
            services.AddScoped<ICompaniesService, CompaniesService>();
            services.AddScoped<ICountriesService, CountriesService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<ICitiesRepository, CityRepository>();
            services.AddScoped<ICitiesService,CitiesService>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();


            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token in the text box below. Example: 'abc123xyz'"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {

                    options.Events = new JwtBearerEvents()
                    {
                        OnChallenge = async (context) =>
                        {
                            context.HandleResponse();

                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";

                            await context.Response.WriteAsJsonAsync(ApiResponse<object>.FailureResponse("Unauthorized. Please login first."));

                        }
                    };
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtConfiguration.Issuer,
                        ValidAudience = jwtConfiguration.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Key))
                    };
                });
            // Ensure authorization services are registered so UseAuthorization works
            services.AddAuthorization();
            return services;
        }
    }
}
