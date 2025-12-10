using System;
using System.Threading;
using System.Threading.Tasks;
using PizzaDeliverySystem.Application.Dtos;
using PizzaDeliverySystem.Application.Dtos.Order;

namespace PizzaDeliverySystem.Application.Contract;

public interface IOrderService
{
    Task<OrderDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<OrderDto> CreateAsync(CreateOrderRequest request, CancellationToken ct = default);

    Task<OrderDto?> ChangeStatusAsync(UpdateOrderStatusRequest request, CancellationToken ct = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
