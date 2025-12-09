using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using PizzaDeliverySystem.Domain.Core.Repository;
using PizzaDeliverySystem.Infrastructure.Models;

namespace PizzaDeliverySystem.Infrastructure.Context;

public class PizzaDbContext : DbContext, IUnitOfWork
{
    public PizzaDbContext(DbContextOptions<PizzaDbContext> options)
        : base(options)
    {
    }

    public DbSet<IngredientModel> Ingredients { get; set; } = null!;
    public DbSet<PizzaModel> Pizzas { get; set; } = null!;
    public DbSet<CustomerModel> Customers { get; set; } = null!;
    public DbSet<OrderModel> Orders { get; set; } = null!;
    public DbSet<OrderItemModel> OrderItems { get; set; } = null!;

    // IUnitOfWork
    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
        => base.SaveChangesAsync(ct);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ejemplo: relación muchos-a-muchos Pizza–Ingredient
        modelBuilder.Entity<PizzaModel>()
            .HasMany(p => p.Ingredients)
            .WithMany(i => i.Pizzas);

        // Ejemplo: relación 1–N Customer–Orders
        modelBuilder.Entity<OrderModel>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId);

        // Ejemplo: relación 1–N Order–OrderItems
        modelBuilder.Entity<OrderItemModel>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId);
    }
}
