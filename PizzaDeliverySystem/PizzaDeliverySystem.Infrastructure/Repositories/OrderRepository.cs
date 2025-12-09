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
                .AsNoTracking()
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
        // Cargar la orden desde el DbContext
        var model = _dbSet.FirstOrDefault(o => o.Id == entity.Id);
        if (model is null)
            throw new OrderRepositoryException($"Order {entity.Id} not found.");

        // Actualizar solo lo necesario
        model.Status = entity.Status;
        model.StatusReason = entity.StatusReason;
        

        // No toques el Id, ni RowVersion, ni cosas raras
    }


    public override void Remove(Order entity)
    {
        try
        {
            //var model = MapToModel(entity);
            var model = new OrderModel { Id = entity.Id };
            //_dbSet.Attach(model);  Redundante debido a que ya hemos trackeado el modelo con .FirstOrDefaultAsync(p => p.Id == id, ct);
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
        var order = new Order(model.Id,model.CustomerId,model.Street, model.City, model.PostalCode);

        foreach (var itemModel in model.Items)
        {
            var orderItem = new OrderItem(itemModel.PizzaId, itemModel.PizzaName, itemModel.Quantity, itemModel.UnitPrice);
            order.AddItem(orderItem);
        }

        return order;
    }

    protected override OrderModel MapToModel(Order entity)
    {
        return new OrderModel
        {
            Id = entity.Id,
            CustomerId = entity.CustomerId,
            Status = entity.Status,
            StatusReason = entity.StatusReason,
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
