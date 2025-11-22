using Microsoft.EntityFrameworkCore;
using PizzaDeliverySystem.Domain.Entities;
using PizzaDeliverySystem.Domain.Core.Repository;
using PizzaDeliverySystem.Infrastructure.Context;
using PizzaDeliverySystem.Infrastructure.Exceptions;
using PizzaDeliverySystem.Infrastructure.Models;

namespace PizzaDeliverySystem.Infrastructure.Repositories;

public class PizzaRepository : BaseRepository<PizzaModel, Pizza>, IPizzaRepository
{
    public PizzaRepository(PizzaDbContext context) : base(context)
    {
    }

    // ---------------- GetById ----------------
    public override async Task<Pizza?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var model = await _dbSet
                .AsNoTracking()
                .Include(p => p.Ingredients)
                .FirstOrDefaultAsync(p => p.Id == id, ct);

            return model is null ? null : MapToDomain(model);
        }
        catch (Exception ex)
        {
            throw new PizzaRepositoryException($"Error getting pizza {id}", ex);
        }
    }

    // ---------------- Add ----------------
    public override async Task AddAsync(Pizza entity, CancellationToken ct = default)
    {
        try
        {
            var model = MapToModel(entity);
            await _dbSet.AddAsync(model, ct);
        }
        catch (Exception ex)
        {
            throw new PizzaRepositoryException("Error adding pizza", ex);
        }
    }

    // ---------------- Update ----------------
    public override void Update(Pizza entity)
    {
        try
        {
            // Cargar la pizza existente con sus ingredientes
            var model = _dbSet
                .Include(p => p.Ingredients)
                .FirstOrDefault(p => p.Id == entity.Id);

            if (model is null)
                throw new PizzaRepositoryException($"Pizza {entity.Id} not found.");

            // Actualizar datos simples
            model.Name = entity.Name;
            model.Size = entity.Size;
            model.BasePrice = entity.BasePrice;

            // Resetear ingredientes y volver a poblar
            model.Ingredients.Clear();

            foreach (var ing in entity.Ingredients)
            {
                var ingModel = new IngredientModel
                {
                    Id = ing.Id,
                    Name = ing.Name,
                    ExtraPrice = ing.ExtraPrice
                };

                model.Ingredients.Add(ingModel);
            }

            // El cambio queda trackeado por el DbContext.
        }
        catch (Exception ex)
        {
            throw new PizzaRepositoryException("Error updating pizza", ex);
        }
    }

    // ---------------- Remove ----------------
    public override void Remove(Pizza entity)
    {
        try
        {
            // Creamos un stub con solo el Id
            var model = new PizzaModel { Id = entity.Id };

            // Lo adjuntamos y marcamos para eliminación
            _dbSet.Attach(model);
            _dbSet.Remove(model);
        }
        catch (Exception ex)
        {
            throw new PizzaRepositoryException("Error removing pizza", ex);
        }
    }


    // ---------------- Mapeos ----------------
    protected override Pizza MapToDomain(PizzaModel model)
    {
        // Hidratas la Pizza con el Id real de la BD
        var pizza = new Pizza(model.Id, model.Name, model.Size, model.BasePrice);

        foreach (var ingModel in model.Ingredients)
        {
            // Hidratas cada ingrediente con su Id real
            var ingredient = new Ingredient(ingModel.Id, ingModel.Name, ingModel.ExtraPrice);
            pizza.AddIngredient(ingredient);
        }

        return pizza;
    }

    protected override PizzaModel MapToModel(Pizza entity)
    {
        return new PizzaModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Size = entity.Size,
            BasePrice = entity.BasePrice,
            Ingredients = entity.Ingredients
                .Select(ing => new IngredientModel
                {
                    Id = ing.Id,
                    Name = ing.Name,
                    ExtraPrice = ing.ExtraPrice
                })
                .ToList()
        };
    }
}
