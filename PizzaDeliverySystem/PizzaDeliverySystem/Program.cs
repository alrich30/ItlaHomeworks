using Microsoft.EntityFrameworkCore;
using PizzaDeliverySystem.Application.Contract;
using PizzaDeliverySystem.Application.Service;
using PizzaDeliverySystem.Domain.Core.Repository;
using PizzaDeliverySystem.Infrastructure.Context;
using PizzaDeliverySystem.Infrastructure.Repositories;
using PizzaDeliverySystem.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
        };
    });

builder.Services.AddAuthorization();

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
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


// 5) Servicios (Aplicación)
builder.Services.AddScoped<IPizzaService, PizzaService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddScoped<IAuthService, AuthService>();


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
