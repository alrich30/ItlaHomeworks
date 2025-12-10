using System.Linq;
using PizzaDeliverySystem.Application.Contract;
using PizzaDeliverySystem.Application.Dtos;
using PizzaDeliverySystem.Domain.Entities;
using PizzaDeliverySystem.Domain.Core.Repository;
//using PizzaDeliverySystem.Infrastructure.Interfaces;


namespace PizzaDeliverySystem.Application.Service;

public class PizzaService : IPizzaService
{
    private readonly IPizzaRepository _pizzaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PizzaService(IPizzaRepository pizzaRepository, IUnitOfWork unitOfWork)
    {
        _pizzaRepository = pizzaRepository;
        _unitOfWork = unitOfWork;
    }

    // ------------------ Consultar ------------------

    public async Task<PizzaDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var pizza = await _pizzaRepository.GetByIdAsync(id, ct);
        return pizza is null ? null : MapToDto(pizza);
    }

    public async Task<IReadOnlyList<PizzaDto>> GetAllAsync(CancellationToken ct = default)
    {
        // Usa el nuevo método del repositorio
        var pizzas = await _pizzaRepository.GetAllAsync(ct);

        // Por si tu IRepository<T> devuelve List<T?>, filtramos nulls
        var list = pizzas
            .Where(p => p is not null)
            .Select(p => MapToDto(p!))
            .ToList();

        return list;
    }

    // ------------------ Crear ------------------

    public async Task<PizzaDto> CreateAsync(CreatePizzaRequest request, CancellationToken ct = default)
    {
        // Validaciones sencillas de aplicación
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required.", nameof(request.Name));

        if (string.IsNullOrWhiteSpace(request.Size))
        {

            throw new ArgumentException("Size is required.", nameof(request.Size));
        }
        

        if (request.BasePrice < 0)
            throw new ArgumentOutOfRangeException(nameof(request.BasePrice), "BasePrice must be >= 0.");

        // Construir entidad de dominio
        var pizza = new Pizza(request.Name, request.Size, request.BasePrice);

        foreach (var ingDto in request.Ingredients)
        {
            var ingredient = new Ingredient(ingDto.Name, ingDto.ExtraPrice);
            pizza.AddIngredient(ingredient);
        }

        // Guardar usando repositorio + unidad de trabajo
        await _pizzaRepository.AddAsync(pizza, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return MapToDto(pizza);
    }

    // ------------------ Actualizar ------------------

    public async Task<PizzaDto?> UpdateAsync(UpdatePizzaRequest request, CancellationToken ct = default)
    {
        var existing = await _pizzaRepository.GetByIdAsync(request.Id, ct);
        if (existing is null)
            return null;

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required.", nameof(request.Name));

        if (string.IsNullOrWhiteSpace(request.Size))
            throw new ArgumentException("Size is required.", nameof(request.Size));

        if (request.BasePrice < 0)
            throw new ArgumentOutOfRangeException(nameof(request.BasePrice));

        // Usamos los métodos de dominio que ya construiste
        existing.SetName(request.Name);
        existing.SetSize(request.Size);
        existing.SetBasePrice(request.BasePrice);

        // Reemplazar ingredientes por los del DTO (estrategia simple)
        var currentIngredients = existing.Ingredients.ToList();
        foreach (var ing in currentIngredients)
            existing.RemoveIngredient(ing.Id);

        foreach (var ingDto in request.Ingredients)
        {
            var ingredient = new Ingredient(ingDto.Name, ingDto.ExtraPrice);
            existing.AddIngredient(ingredient);
        }

        _pizzaRepository.Update(existing);
        await _unitOfWork.SaveChangesAsync(ct);

        return MapToDto(existing);
    }

    // ------------------ Eliminar ------------------

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var existing = await _pizzaRepository.GetByIdAsync(id, ct);
        if (existing is null)
            return false;

        _pizzaRepository.Remove(existing);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    // ------------------ Mapping entidad -> DTO ------------------

    private static PizzaDto MapToDto(Pizza entity)
    {
        return new PizzaDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Size = entity.Size,
            BasePrice = entity.BasePrice,
            Ingredients = entity.Ingredients
                .Select(i => new IngredientDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    ExtraPrice = i.ExtraPrice
                })
                .ToList()
        };
    }
}
