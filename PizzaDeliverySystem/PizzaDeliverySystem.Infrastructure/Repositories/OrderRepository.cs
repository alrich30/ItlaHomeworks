using Microsoft.EntityFrameworkCore;
using PizzaDeliverySystem.Domain.Entities;
using PizzaDeliverySystem.Infrastructure.Context;
using PizzaDeliverySystem.Infrastructure.Core;
using PizzaDeliverySystem.Infrastructure.Exceptions;
using PizzaDeliverySystem.Infrastructure.Interfaces;
using PizzaDeliverySystem.Infrastructure.Models;

namespace PizzaDeliverySystem.Infrastructure.Repositories;

public class OrderRepository
    : BaseRepository<OrderModel, Order>, IOrderRepository
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
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == id, ct);

            if (model is null) return null;

            // TODO: mapear OrderModel -> Order
            throw new NotImplementedException("Map OrderModel -> Order aún no implementado.");
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
            // TODO: mapear Order -> OrderModel
            throw new NotImplementedException("Map Order -> OrderModel aún no implementado.");
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
            // TODO: actualizar modelo
            throw new NotImplementedException("Update Order aún no implementado.");
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
            // TODO: eliminar modelo
            throw new NotImplementedException("Remove Order aún no implementado.");
        }
        catch (Exception ex)
        {
            throw new OrderRepositoryException("Error removing order", ex);
        }
    }
}

