using Microsoft.EntityFrameworkCore;
using PizzaDeliverySystem.Domain.Core;
using PizzaDeliverySystem.Domain.Core.Errors;
using PizzaDeliverySystem.Domain.Core.Repository;
using PizzaDeliverySystem.Infrastructure.Context;

namespace PizzaDeliverySystem.Infrastructure.Core;

public abstract class BaseRepository<TModel, TAggregate> : IRepository<TAggregate>
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

    // ------------- Contrato IRepository<TAggregate> -------------

    public abstract Task<TAggregate?> GetByIdAsync(Guid id, CancellationToken ct = default);

    public abstract Task AddAsync(TAggregate entity, CancellationToken ct = default);

    public abstract void Update(TAggregate entity);

    public abstract void Remove(TAggregate entity);

    // ------------- Helper para errores de mapeo -------------

    protected DomainException MappingError(string message)
        => new DomainException($"Mapping error in {GetType().Name}: {message}");
}
