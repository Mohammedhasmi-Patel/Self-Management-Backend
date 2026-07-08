using Microsoft.AspNetCore.Mvc;
using SelfManagement.API.Common;

namespace SelfManagement.API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services,IConfiguration configuration)
        {
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
            return services;
        }
    }
}
