using Microsoft.EntityFrameworkCore;
using PizzaDeliverySystem.Domain.Core;
using PizzaDeliverySystem.Domain.Core.Repository;
using PizzaDeliverySystem.Domain.Entities;
using PizzaDeliverySystem.Infrastructure.Context;
using PizzaDeliverySystem.Infrastructure.Models;

namespace PizzaDeliverySystem.Infrastructure.Repositories;

public abstract class BaseRepository<TModel, TEntity> : IRepository<TEntity>
    where TModel : class
    where TEntity : BaseEntity, IAggregateRoot
{
    protected readonly PizzaDbContext _context;
    protected readonly DbSet<TModel> _dbSet;

    protected BaseRepository(PizzaDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TModel>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var model = await _dbSet.FindAsync(new object?[] { id }, ct);
        return model is null ? null : MapToDomain(model);
    }

    // ---------------- GET ALL ----------------
    public virtual async Task<List<TEntity>> GetAllAsync(CancellationToken ct = default)
    {
        var models = await _dbSet
            .AsNoTracking()
            .ToListAsync(ct);

        return models
            .Select(MapToDomain)
            .ToList();
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken ct = default)
    {
        var model = MapToModel(entity);
        await _dbSet.AddAsync(model, ct);
    }

    public virtual void Remove(TEntity entity)
    {
        var model = MapToModel(entity);
        _dbSet.Remove(model);
    }

    public abstract void Update(TEntity entity);

    // Métodos que cada repositorio concreto implementará
    protected abstract TEntity MapToDomain(TModel model);
    protected abstract TModel MapToModel(TEntity entity);
}
