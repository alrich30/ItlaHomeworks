using Microsoft.EntityFrameworkCore;
using PizzaDeliverySystem.Domain.Entities;
using PizzaDeliverySystem.Infrastructure.Context;
using PizzaDeliverySystem.Infrastructure.Core;
using PizzaDeliverySystem.Infrastructure.Interfaces;
using PizzaDeliverySystem.Infrastructure.Models;

namespace PizzaDeliverySystem.Infrastructure.Repositories;

public class CustomerRepository
    : BaseRepository<CustomerModel, Customer>, ICustomerRepository
{
    public CustomerRepository(PizzaDbContext context) : base(context)
    {
    }

    public override async Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            // Si quieres incluir pedidos del cliente:
            // var model = await _dbSet
            //     .Include(c => c.Orders)
            //     .FirstOrDefaultAsync(c => c.Id == id, ct);

            var model = await _dbSet.FirstOrDefaultAsync(c => c.Id == id, ct);

            if (model is null)
                return null;

            var customer = MapToDomain(model);
            return customer;
        }
        catch (Exception ex)
        {
            // Aquí podrías crear una CustomerRepositoryException si quisieras
            throw new Exception($"Error getting customer {id}", ex);
        }
    }

    public override async Task AddAsync(Customer entity, CancellationToken ct = default)
    {
        try
        {
            var model = MapToModel(entity);
            await _dbSet.AddAsync(model, ct);
            // SaveChangesAsync lo hará IUnitOfWork desde la capa de aplicación.
        }
        catch (Exception ex)
        {
            throw new Exception("Error adding customer", ex);
        }
    }

    public override void Update(Customer entity)
    {
        try
        {
            var model = _dbSet.FirstOrDefault(c => c.Id == entity.Id);
            if (model is null)
                throw new Exception($"Customer {entity.Id} not found for update.");

            model.FullName = entity.FullName;
            model.Phone = entity.Phone;
            model.Street = entity.Street;
            model.City = entity.City;
            model.PostalCode = entity.PostalCode;
            // EF Core rastrea el modelo, no hace falta más aquí.
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating customer", ex);
        }
    }

    public override void Remove(Customer entity)
    {
        try
        {
            var model = new CustomerModel { Id = entity.Id };
            _context.Attach(model);
            _dbSet.Remove(model);
        }
        catch (Exception ex)
        {
            throw new Exception("Error removing customer", ex);
        }
    }

    // ------------- Mapping helpers -------------

    private static Customer MapToDomain(CustomerModel model)
    {
        // Asumiendo un constructor:
        // Customer(string fullName, string phone, string street, string city, string postalCode)
        var customer = new Customer(
            model.FullName,
            model.Phone,
            model.Street,
            model.City,
            model.PostalCode
        );

        // Igual que con Pizza: el Id de la BD no se está rehidratando
        // porque tu BaseEntity genera Id automáticamente.
        // Si quisieras, se puede mejorar el modelo de dominio más adelante.

        return customer;
    }

    private static CustomerModel MapToModel(Customer entity)
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
