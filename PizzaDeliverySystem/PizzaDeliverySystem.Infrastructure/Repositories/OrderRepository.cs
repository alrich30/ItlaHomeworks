using Microsoft.EntityFrameworkCore;
using PizzaDeliverySystem.Domain.Entities;
using PizzaDeliverySystem.Domain.Core.Repository;
using PizzaDeliverySystem.Infrastructure.Context;
using PizzaDeliverySystem.Infrastructure.Exceptions;
using PizzaDeliverySystem.Infrastructure.Models;

namespace PizzaDeliverySystem.Infrastructure.Repositories;

public class OrderRepository : BaseRepository<OrderModel, Order>, IOrderRepository
{
    public OrderRepository(PizzaDbContext context) : base(context)
    {
    }

    public override async Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var model = await _dbSet
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id, ct);

            return model is null ? null : MapToDomain(model);
        }
        catch (Exception ex)
        {
            throw new OrderRepositoryException($"Error getting order {id}", ex);
        }
    }

    public override async Task AddAsync(Order entity, CancellationToken ct = default)
    {
        try
        {
            var model = MapToModel(entity);
            await _dbSet.AddAsync(model, ct);
        }
        catch (Exception ex)
        {
            throw new OrderRepositoryException("Error adding order", ex);
        }
    }

    public override void Update(Order entity)
    {
        try
        {
            var model = _dbSet
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == entity.Id);

            if (model is null)
                throw new OrderRepositoryException($"Order {entity.Id} not found.");

            model.CustomerId = entity.CustomerId;
            model.Status = entity.Status;
            model.Street = entity.Street;
            model.City = entity.City;
            model.PostalCode = entity.PostalCode;

            // Actualizar items: estrategia simple = borrar y recrear
            model.Items.Clear();

            foreach (var item in entity.Items)
            {
                var itemModel = new OrderItemModel
                {
                    Id = item.Id,
                    OrderId = model.Id,
                    PizzaId = item.PizzaId,
                    PizzaName = item.PizzaName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };

                model.Items.Add(itemModel);
            }
        }
        catch (Exception ex)
        {
            throw new OrderRepositoryException("Error updating order", ex);
        }
    }

    public override void Remove(Order entity)
    {
        try
        {
            var model = MapToModel(entity);
            _dbSet.Remove(model);
        }
        catch (Exception ex)
        {
            throw new OrderRepositoryException("Error removing order", ex);
        }
    }

    // ---------------- Mapeos ----------------
    protected override Order MapToDomain(OrderModel model)
    {
        var order = new Order(
            model.CustomerId,model.Street, model.City, model.PostalCode // o tu versión aplanada
        );

        // similar tema de Id / items aquí, según tu diseño de Order.

        return order;
    }

    protected override OrderModel MapToModel(Order entity)
    {
        return new OrderModel
        {
            Id = entity.Id,
            CustomerId = entity.CustomerId,
            Status = entity.Status,
            Street = entity.Street,
            City = entity.City,
            PostalCode = entity.PostalCode,
            Items = entity.Items
                .Select(i => new OrderItemModel
                {
                    Id = i.Id,
                    OrderId = entity.Id,
                    PizzaId = i.PizzaId,
                    PizzaName = i.PizzaName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                })
                .ToList()
        };
    }
}
