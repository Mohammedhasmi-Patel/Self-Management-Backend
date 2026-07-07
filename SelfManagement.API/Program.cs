using SelfManagement.API.Extensions;
using SelfManagement.API.Middleware;
using SelfManagement.API.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDependencyInjection();
builder.Services.Configure<EmailSetting>(options =>
{
    builder.Configuration.GetSection("EmailSettings").Bind(options);

    options.TemplatePath = Path.Combine(
        builder.Environment.ContentRootPath,
        "EmailTemplates");
});
builder.Services.AddDatabase(builder.Configuration);


var app = builder.Build();
app.UseGlobalExceptionMiddleware();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
