using Microsoft.EntityFrameworkCore;
using PizzaDeliverySystem.Domain.Entities;
using PizzaDeliverySystem.Infrastructure.Context;
using PizzaDeliverySystem.Infrastructure.Core;
using PizzaDeliverySystem.Infrastructure.Exceptions;
using PizzaDeliverySystem.Infrastructure.Interfaces;
using PizzaDeliverySystem.Infrastructure.Models;

namespace PizzaDeliverySystem.Infrastructure.Repositories;

public class PizzaRepository
    : BaseRepository<PizzaModel, Pizza>, IPizzaRepository
{
    public PizzaRepository(PizzaDbContext context) : base(context)
    {
    }

    // ----------------- IRepository<Pizza> -----------------

    public override async Task<Pizza?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var model = await _dbSet
                .Include(p => p.Ingredients)
                .FirstOrDefaultAsync(p => p.Id == id, ct);

            if (model is null) return null;

            // TODO: mapear PizzaModel -> Pizza (dominio)
            throw new NotImplementedException("Map PizzaModel -> Pizza aún no implementado.");
        }
        catch (Exception ex)
        {
            throw new PizzaRepositoryException($"Error getting pizza {id}", ex);
        }
    }

    public override async Task AddAsync(Pizza entity, CancellationToken ct = default)
    {
        try
        {
            // TODO: mapear Pizza (dominio) -> PizzaModel (infra)
            throw new NotImplementedException("Map Pizza -> PizzaModel aún no implementado.");
        }
        catch (Exception ex)
        {
            throw new PizzaRepositoryException("Error adding pizza", ex);
        }
    }

    public override void Update(Pizza entity)
    {
        try
        {
            // TODO: buscar modelo existente y mapear cambios
            throw new NotImplementedException("Update Pizza aún no implementado.");
        }
        catch (Exception ex)
        {
            throw new PizzaRepositoryException("Error updating pizza", ex);
        }
    }

    public override void Remove(Pizza entity)
    {
        try
        {
            // TODO: mapear / adjuntar modelo y marcar para eliminar
            throw new NotImplementedException("Remove Pizza aún no implementado.");
        }
        catch (Exception ex)
        {
            throw new PizzaRepositoryException("Error removing pizza", ex);
        }
    }
}
