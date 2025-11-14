using Microsoft.EntityFrameworkCore;
using PizzaDeliverySystem.Domain.Core.Repository;
using PizzaDeliverySystem.Domain.Core;
using PizzaDeliverySystem.Domain.Core.Errors;
using PizzaDeliverySystem.Infrastructure.Context;

namespace PizzaDeliverySystem.Infrastructure.Core;

public abstract class BaseRepository<TModel, TAggregate>
    where TModel : class
    where TAggregate : BaseEntity, IAggregateRoot
{
    protected readonly PizzaDbContext _context;
    protected readonly DbSet<TModel> _dbSet;

    protected BaseRepository(PizzaDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TModel>();
    }

    // Más adelante aquí harás el mapeo Model <-> Entidad de Dominio.
    // Por ahora solo te dejo la estructura que suele usarse.

    protected DomainException MappingError(string message)
        => new DomainException($"Mapping error in {GetType().Name}: {message}");
}
