using SelfManagement.API.Extensions;
using SelfManagement.API.Middleware;
using SelfManagement.Infrastructure.Seeder;

var builder = WebApplication.CreateBuilder(args);

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
app.UseGlobalExceptionMiddleware();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();


app.Run();
