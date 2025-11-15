using Microsoft.EntityFrameworkCore;
using PizzaDeliverySystem.Domain.Core.Errors;
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

            if (model is null)
                return null;

            var order = MapToDomain(model);
            return order;
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
            // SaveChangesAsync lo hará IUnitOfWork.
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
                throw new OrderRepositoryException($"Order {entity.Id} not found for update.");

            // Actualizar datos básicos
            model.CustomerId = entity.CustomerId;
            model.Status = entity.Status;
            model.Street = entity.Street;
            model.City = entity.City;
            model.PostalCode = entity.PostalCode;

            // Reseteamos items y los volvemos a poblar según el dominio
            model.Items.Clear();

            foreach (var item in entity.Items)
            {
                var itemModel = new OrderItemModel
                {
                    Id = item.Id,
                    OrderId = entity.Id,
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
            var model = new OrderModel { Id = entity.Id };
            _context.Attach(model);
            _dbSet.Remove(model);
        }
        catch (Exception ex)
        {
            throw new OrderRepositoryException("Error removing order", ex);
        }
    }

    // ------------- Mapping helpers -------------

    private static Order MapToDomain(OrderModel model)
    {
        // Asumiendo constructor:
        // Order(Guid customerId, string street, string city, string postalCode)

        var order = new Order(
            model.CustomerId,
            model.Street,
            model.City,
            model.PostalCode
        );

        // Primero mapear items mientras el estado sigue en "Created"
        foreach (var itemModel in model.Items)
        {
            var item = new OrderItem(
                itemModel.PizzaId,
                itemModel.PizzaName,
                itemModel.Quantity,
                itemModel.UnitPrice
            );

            order.AddItem(item);
        }

        // Luego ajustar el estado según el Status almacenado
        // Aquí asumiré los estados posibles como strings:
        // "Created", "Confirmed", "InKitchen", "OutForDelivery", "Delivered", "Cancelled"

        switch (model.Status)
        {
            case "Created":
                // nada que hacer, ya está creado así
                break;

            case "Confirmed":
                order.Confirm();
                break;

            case "InKitchen":
                order.Confirm();
                order.StartKitchen();
                break;

            case "OutForDelivery":
                order.Confirm();
                order.StartKitchen();
                order.StartDelivery();
                break;

            case "Delivered":
                order.Confirm();
                order.StartKitchen();
                order.StartDelivery();
                order.Deliver();
                break;

            case "Cancelled":
                // No sabemos la razón real, puedes usar un texto genérico
                order.Cancel("Loaded from persistence.");
                break;

            default:
                throw new DomainException($"Unknown order status '{model.Status}' when mapping from database.");
        }

        return order;
    }

    private static OrderModel MapToModel(Order entity)
    {
        var model = new OrderModel
        {
            Id = entity.Id,
            CustomerId = entity.CustomerId,
            Status = entity.Status,
            Street = entity.Street,
            City = entity.City,
            PostalCode = entity.PostalCode,
            Items = entity.Items
                .Select(item => new OrderItemModel
                {
                    Id = item.Id,
                    OrderId = entity.Id,
                    PizzaId = item.PizzaId,
                    PizzaName = item.PizzaName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                })
                .ToList()
        };

        return model;
    }
}
