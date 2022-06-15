using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TLM.Books._Infrastructure;
using TLM.Books.API.Middlewares;
using TLM.Books.Application;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfra(configuration);
builder.Services.AddApplication();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
var environmentName = builder.Environment.EnvironmentName;
builder.Configuration.AddJsonFile($"appsettings.{environmentName}.json");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<BookDbContext>();
        db.Database.Migrate();
    }
}



// app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();

public partial class Program { }