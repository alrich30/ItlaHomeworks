using AlPelis.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext (MySQL)
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MainConnection"),
        new MySqlServerVersion(new Version(8, 0, 30))
    )
);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
