using Microsoft.EntityFrameworkCore;
using PizzaDeliverySystem.Application.Contract;
using PizzaDeliverySystem.Application.Service;
using PizzaDeliverySystem.Domain.Core.Repository;
using PizzaDeliverySystem.Infrastructure.Context;
using PizzaDeliverySystem.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1) Controllers
builder.Services.AddControllers();

// 2) DbContext (MySQL / MariaDB con Pomelo.EntityFrameworkCore.MySql)
builder.Services.AddDbContext<PizzaDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("PizzaDb"),
        new MySqlServerVersion(new Version(8, 0, 36))
    )
);

// 3) Unit of Work
builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<PizzaDbContext>());

// 4) Repositorios (Infraestructura)
builder.Services.AddScoped<IPizzaRepository, PizzaRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// 5) Servicios (Aplicación)
builder.Services.AddScoped<IPizzaService, PizzaService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// 6) CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed(origin => true); // Permite todos los orígenes (útil en desarrollo)
        });
});



var app = builder.Build();

app.UseCors("AllowFrontend");

// Middlewares básicos
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
