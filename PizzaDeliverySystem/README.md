# 🍕 PizzaDeliverySystem

A pizza delivery REST API built with **C#**, **ASP.NET Core 8**, **Entity Framework Core** and **MySQL**.

The project separates HTTP endpoints, application services, domain entities and data persistence. It was developed as an educational backend project to practice API design, database relationships and business rules.

**Resumen en español:** API para gestionar pizzas, clientes y pedidos. Desarrollada con C# y ASP.NET Core 8 como proyecto de práctica de backend.

## Technologies

- C# and .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- MySQL and Pomelo.EntityFrameworkCore.MySql
- JWT for authentication tokens

## Project structure

```text
PizzaDeliverySystem/
├── PizzaDeliverySystem/                 # API and controllers
├── PizzaDeliverySystem.Application/     # Services, interfaces and DTOs
├── PizzaDeliverySystem.Domain/          # Domain entities and business rules
├── PizzaDeliverySystem.Infrastructure/  # EF Core context, repositories and migrations
└── Client-js/                           # JavaScript client
```

## What the API does

- Creates, lists, retrieves, updates and deletes pizzas.
- Creates, retrieves, updates and deletes customers.
- Creates, lists, retrieves and deletes orders.
- Changes an order's status through a dedicated endpoint. The domain defines the statuses `Created`, `Confirmed`, `InKitchen`, `OutForDelivery`, `Delivered` and `Cancelled`.
- Includes endpoints for user registration and login that return JWT tokens.

## Run locally

### Requirements

- .NET 8 SDK
- MySQL 8
- EF Core command-line tools, if you plan to apply migrations with `dotnet ef`

### 1. Clone the repository

```bash
git clone https://github.com/alrich30/ItlaHomeworks.git
cd ItlaHomeworks/PizzaDeliverySystem
```

### 2. Configure the API

The repository does not currently include an `appsettings.json` file for this project. Create one locally at `PizzaDeliverySystem/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "PizzaDbV2": "server=localhost;database=pizzadb;user=YOUR_USER;password=YOUR_PASSWORD;"
  },
  "Jwt": {
    "Key": "REPLACE_WITH_A_LONG_RANDOM_SECRET_KEY",
    "Issuer": "PizzaDeliverySystem",
    "Audience": "PizzaDeliverySystem.Client"
  }
}
```

Use your own database credentials and a long, random JWT key. **Do not commit this file with real credentials or secrets.** Add its path to the repository's `.gitignore` before saving local secrets.

### 3. Restore dependencies and apply the existing migration

```bash
dotnet restore PizzaDeliverySystem/PizzaDeliverySystem.csproj

dotnet ef database update \
  --project PizzaDeliverySystem.Infrastructure/PizzaDeliverySystem.Infrastructure.csproj \
  --startup-project PizzaDeliverySystem/PizzaDeliverySystem.csproj
```

The connection must point to a MySQL server your user can access. The existing migration creates the main pizza delivery tables.

### 4. Start the API

```bash
dotnet run --project PizzaDeliverySystem/PizzaDeliverySystem.csproj
```

Use the HTTP or HTTPS address shown in the terminal to call the endpoints below.

## API endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/api/pizza` | List pizzas |
| GET | `/api/pizza/{id}` | Get a pizza |
| POST | `/api/pizza` | Create a pizza |
| PUT | `/api/pizza/{id}` | Update a pizza |
| DELETE | `/api/pizza/{id}` | Delete a pizza |
| GET | `/api/customer/{id}` | Get a customer |
| POST | `/api/customer` | Create a customer |
| PUT | `/api/customer/{id}` | Update a customer |
| DELETE | `/api/customer/{id}` | Delete a customer |
| GET | `/api/order` | List orders |
| GET | `/api/order/{id}` | Get an order |
| POST | `/api/order` | Create an order |
| PUT | `/api/order/{id}/status` | Change an order's status |
| DELETE | `/api/order/{id}` | Delete an order |
| POST | `/api/auth/register` | Register a user |
| POST | `/api/auth/login` | Log in |

### Example: create an order

`POST /api/order`

```json
{
  "customerId": "2666dff5-3848-441f-a00c-0ef03c3723fd",
  "street": "Main Street 123",
  "city": "Santo Domingo",
  "postalCode": "10101",
  "items": [
    {
      "pizzaId": "56df2dc3-2bda-47b1-ab06-e7296a775e10",
      "pizzaName": "Hawaiian",
      "quantity": 2,
      "unitPrice": 12.5
    }
  ]
}
```

The customer and pizza IDs must refer to existing records.

### Example: change an order's status

`PUT /api/order/{id}/status`

```json
{
  "newStatus": "Confirmed"
}
```

## Current limitation

The committed database migration does not include the `Users` table used by the authentication code. Registration and login need an additional migration before they can work with a fresh database.

## Author

**Richard Torres** — Software Development student  
GitHub: [@alrich30](https://github.com/alrich30)
