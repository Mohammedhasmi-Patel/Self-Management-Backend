using SelfManagement.API.Extensions;
using SelfManagement.API.Middleware;
using SelfManagement.Infrastructure.Seeder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register database first so Identity can configure EF stores correctly
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddDependencyInjection(builder.Configuration);


var app = builder.Build();
// Configure the HTTP request pipeline.
await DatabaseSeeder.SeedAsync(app.Services);


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseGlobalExceptionMiddleware();

app.MapControllers();


app.Run();
