using SelfManagement.API.Extensions;
using System.Net;
using System.Text.Json;
using SelfManagement.Application.Exceptions;
using SelfManagement.Application.DTO.Common;
namespace SelfManagement.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception processing request {Method} {Path}", context.Request.Method, context.Request.Path);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            int statusCode;
            string message;

            switch (ex)
            {
                case AppException appException:
                    statusCode = appException.StatusCode;
                    message = appException.Message;
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    message = "Something went wrong.";
                    break;
            }

            context.Response.StatusCode = statusCode;

            var response = ApiResponse<object>.FailureResponse(message);

            return context.Response.WriteAsJsonAsync(response);
        }
    }
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
