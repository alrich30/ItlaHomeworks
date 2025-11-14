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
        // TODO: implementar mapeo real
        throw new NotImplementedException("GetByIdAsync Customer aún no implementado.");
    }

    public override async Task AddAsync(Customer entity, CancellationToken ct = default)
    {
        // TODO: implementar mapeo real
        throw new NotImplementedException("AddAsync Customer aún no implementado.");
    }

    public override void Update(Customer entity)
    {
        // TODO: implementar
        throw new NotImplementedException("Update Customer aún no implementado.");
    }

    public override void Remove(Customer entity)
    {
        // TODO: implementar
        throw new NotImplementedException("Remove Customer aún no implementado.");
    }
}
