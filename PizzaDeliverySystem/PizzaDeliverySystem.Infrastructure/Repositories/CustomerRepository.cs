using Microsoft.EntityFrameworkCore;
using PizzaDeliverySystem.Domain.Entities;
using PizzaDeliverySystem.Domain.Core.Repository;
using PizzaDeliverySystem.Infrastructure.Context;
using PizzaDeliverySystem.Infrastructure.Exceptions;
using PizzaDeliverySystem.Infrastructure.Models;

namespace PizzaDeliverySystem.Infrastructure.Repositories;

public class CustomerRepository : BaseRepository<CustomerModel, Customer>, ICustomerRepository
{
    public CustomerRepository(PizzaDbContext context) : base(context)
    {
    }

    public override async Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var model = await _dbSet
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id == id, ct);

            return model is null ? null : MapToDomain(model);
        }
        catch (Exception ex)
        {
            throw new CustomerRepositoryException($"Error getting customer {id}", ex);
        }
    }

    public override async Task AddAsync(Customer entity, CancellationToken ct = default)
    {
        try
        {
            var model = MapToModel(entity);
            await _dbSet.AddAsync(model, ct);
        }
        catch (Exception ex)
        {
            throw new CustomerRepositoryException("Error adding customer", ex);
        }
    }

    public override void Update(Customer entity)
    {
        try
        {
            var model = _dbSet.FirstOrDefault(c => c.Id == entity.Id);
            if (model is null)
                throw new CustomerRepositoryException($"Customer {entity.Id} not found.");

            model.FullName = entity.FullName;
            model.Phone = entity.Phone;
            model.Street = entity.Street;
            model.City = entity.City;
            model.PostalCode = entity.PostalCode;

            // Si quisieras actualizar órdenes también, se haría aquí.
        }
        catch (Exception ex)
        {
            throw new CustomerRepositoryException("Error updating customer", ex);
        }
    }

    public override void Remove(Customer entity)
    {
        try
        {
            var model = MapToModel(entity);
            _dbSet.Remove(model);
        }
        catch (Exception ex)
        {
            throw new CustomerRepositoryException("Error removing customer", ex);
        }
    }

    protected override Customer MapToDomain(CustomerModel model)
    {
        var customer = new Customer(
            model.FullName,
            model.Phone,
            model.Street,
            model.City,
            model.PostalCode
        );

        // Igual que antes, en un sistema real hidrataríamos Ids y Orders.

        return customer;
    }

    protected override CustomerModel MapToModel(Customer entity)
    {
        return new CustomerModel
        {
            Id = entity.Id,
            FullName = entity.FullName,
            Phone = entity.Phone,
            Street = entity.Street,
            City = entity.City,
            PostalCode = entity.PostalCode
        };
    }
}
