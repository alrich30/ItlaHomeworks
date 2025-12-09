using System;
using System.Threading;
using System.Threading.Tasks;
using PizzaDeliverySystem.Application.Dtos;

namespace PizzaDeliverySystem.Application.Contract;

public interface ICustomerService
{
    Task<CustomerDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<CustomerDto> CreateAsync(CreateCustomerRequest request, CancellationToken ct = default);

    Task<CustomerDto?> UpdateAsync(UpdateCustomerRequest request, CancellationToken ct = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
