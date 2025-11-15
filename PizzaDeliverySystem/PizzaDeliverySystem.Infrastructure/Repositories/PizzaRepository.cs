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

            if (model is null)
                return null;

            // Mapeo PizzaModel -> Pizza (dominio)
            var pizza = MapToDomain(model);
            return pizza;
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
            var model = MapToModel(entity);
            await _dbSet.AddAsync(model, ct);
            // IMPORTANTE: aquí NO llamamos SaveChangesAsync.
            // Eso lo hará IUnitOfWork.SaveChangesAsync() desde la capa de aplicación.
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
            // Cargamos el modelo actual desde la BD
            var model = _dbSet
                .Include(p => p.Ingredients)
                .FirstOrDefault(p => p.Id == entity.Id);

            if (model is null)
                throw new PizzaRepositoryException($"Pizza {entity.Id} not found for update.");

            // Actualizamos propiedades básicas
            model.Name      = entity.Name;
            model.Size      = entity.Size;
            model.BasePrice = entity.BasePrice;

            // Para simplificar, reseteamos la lista de ingredientes y la volvemos a poblar
            model.Ingredients.Clear();

            foreach (var ing in entity.Ingredients)
            {
                // OJO: aquí estamos creando nuevos IngredientModel.
                // Si quisieras reutilizar ingredientes existentes, deberías buscarlos
                // desde el contexto en vez de crear siempre nuevos.
                var ingredientModel = new IngredientModel
                {
                    Id         = ing.Id,
                    Name       = ing.Name,
                    ExtraPrice = ing.ExtraPrice
                };

                model.Ingredients.Add(ingredientModel);
            }

            // EF Core ya rastrea 'model', así que basta con esto para marcar cambios.
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
            // Opción simple: adjuntar un stub y marcarlo para eliminar
            var model = new PizzaModel { Id = entity.Id };
            _context.Attach(model);
            _dbSet.Remove(model);
        }
        catch (Exception ex)
        {
            throw new PizzaRepositoryException("Error removing pizza", ex);
        }
    }

    // ----------------- MAPEOS PRIVADOS -----------------

    private static Pizza MapToDomain(PizzaModel model)
    {
        // Crear la entidad de dominio
        var pizza = new Pizza(
            model.Name,
            model.Size,
            model.BasePrice
        );

        // IMPORTANTE:
        // Aquí estamos perdiendo el Id original del modelo,
        // porque la entidad Pizza genera su propio Id en BaseEntity.
        // Para un proyecto real, idealmente tendrías un constructor
        // o método interno que permita asignar el Id desde la persistencia.

        // Mapear ingredientes
        foreach (var ingModel in model.Ingredients)
        {
            var ingredient = new Ingredient(
                ingModel.Name,
                ingModel.ExtraPrice
            );

            // Igual que con Pizza, el Id del Ingredient de dominio
            // se generará nuevo; en un sistema real también se hidrataría Id.
            pizza.AddIngredient(ingredient);
        }

        return pizza;
    }

    private static PizzaModel MapToModel(Pizza entity)
    {
        var model = new PizzaModel
        {
            Id         = entity.Id,      // Aquí sí aprovechamos el Id que viene del dominio
            Name       = entity.Name,
            Size       = entity.Size,
            BasePrice  = entity.BasePrice,
            Ingredients = entity.Ingredients
                .Select(ing => new IngredientModel
                {
                    Id         = ing.Id,
                    Name       = ing.Name,
                    ExtraPrice = ing.ExtraPrice
                })
                .ToList()
        };

        return model;
    }
}
