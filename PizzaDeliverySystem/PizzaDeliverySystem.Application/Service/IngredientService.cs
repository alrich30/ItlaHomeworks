using PizzaDeliverySystem.Application.Contract;
using PizzaDeliverySystem.Application.Dtos.Ingredient;
using PizzaDeliverySystem.Domain.Core.Repository;
using PizzaDeliverySystem.Domain.Entities;

namespace PizzaDeliverySystem.Application.Service;

public class IngredientService : IIngredientService
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public IngredientService(
        IIngredientRepository ingredientRepository,
        IUnitOfWork unitOfWork)
    {
        _ingredientRepository = ingredientRepository;
        _unitOfWork = unitOfWork;
    }

    // -------- Get by Id --------
    public async Task<IngredientDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var ingredient = await _ingredientRepository.GetByIdAsync(id, ct);
        return ingredient is null ? null : MapToDto(ingredient);
    }


    // -------- Create --------
    public async Task<IngredientDto> CreateAsync(CreateIngredientRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required.", nameof(request.Name));

        if (request.ExtraPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(request.ExtraPrice));

        // Podrías validar aquí nombres duplicados si quieres
        var ingredient = new Ingredient(request.Name, request.ExtraPrice);

        await _ingredientRepository.AddAsync(ingredient, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return MapToDto(ingredient);
    }

    // -------- Update --------
    public async Task<IngredientDto?> UpdateAsync(Guid id, UpdateIngredientRequest request, CancellationToken ct = default)
    {
        if (id != request.Id)
            throw new ArgumentException("Route id and body id must match.", nameof(id));

        var existing = await _ingredientRepository.GetByIdAsync(id, ct);
        if (existing is null)
            return null;

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required.", nameof(request.Name));

        if (request.ExtraPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(request.ExtraPrice));

        // Métodos de dominio (implementa algo similar en Ingredient)
        existing.SetName(request.Name);
        existing.SetExtraPrice(request.ExtraPrice);

        _ingredientRepository.Update(existing);
        await _unitOfWork.SaveChangesAsync(ct);

        return MapToDto(existing);
    }

    // -------- Delete --------
    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var existing = await _ingredientRepository.GetByIdAsync(id, ct);
        if (existing is null)
            return false;

        _ingredientRepository.Remove(existing);
        await _unitOfWork.SaveChangesAsync(ct);

        return true;
    }

    // -------- Mapping --------
    private static IngredientDto MapToDto(Ingredient ingredient)
        => new IngredientDto
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            ExtraPrice = ingredient.ExtraPrice
        };
}