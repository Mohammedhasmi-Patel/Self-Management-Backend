using SelfManagement.API.Extensions;
using SelfManagement.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDependencyInjection(builder.Configuration);
builder.Services.AddDatabase(builder.Configuration);


var app = builder.Build();
app.UseGlobalExceptionMiddleware();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
