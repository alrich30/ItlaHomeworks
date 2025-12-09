using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PizzaDeliverySystem.Application.Dtos.Pizza;

namespace PizzaDeliverySystem.Application.Contract;

public interface IPizzaService
{
    Task<PizzaDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<IReadOnlyList<PizzaDto>> GetAllAsync(CancellationToken ct = default);

    Task<PizzaDto> CreateAsync(CreatePizzaRequest request, CancellationToken ct = default);

    Task<PizzaDto?> UpdateAsync(Guid id, UpdatePizzaRequest request, CancellationToken ct = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
