using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PizzaDeliverySystem.Application.Contract;
using PizzaDeliverySystem.Application.Dtos;
using PizzaDeliverySystem.Application.Dtos.Order;
using PizzaDeliverySystem.Domain.Core.Repository;
using PizzaDeliverySystem.Domain.Entities;

namespace PizzaDeliverySystem.Application.Service;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPizzaRepository _pizzaRepository;     // opcional: validar pizzas
    private readonly ICustomerRepository _customerRepository; // opcional: validar cliente
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(
        IOrderRepository orderRepository,
        IPizzaRepository pizzaRepository,
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _pizzaRepository = pizzaRepository;
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    // ------------------ Consultar ------------------

    public async Task<OrderDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, ct);
        return order is null ? null : MapToDto(order);
    }

    // ------------------ Crear ------------------

    public async Task<OrderDto> CreateAsync(CreateOrderRequest request, CancellationToken ct = default)
    {
        // Validaciones de aplicación
        if (request.CustomerId == Guid.Empty)
            throw new ArgumentException("CustomerId is required.", nameof(request.CustomerId));

        if (request.Items is null || !request.Items.Any())
            throw new ArgumentException("Order must contain at least one item.", nameof(request.Items));

        if (string.IsNullOrWhiteSpace(request.Street) ||
            string.IsNullOrWhiteSpace(request.City) ||
            string.IsNullOrWhiteSpace(request.PostalCode))
            throw new ArgumentException("Full delivery address is required.");

        // (Opcional) validar que el cliente exista
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, ct);
        if (customer is null)
            throw new InvalidOperationException("Customer does not exist.");

        // Construir entidad de dominio
        var order = new Order(
            request.CustomerId,
            request.Street,
            request.City,
            request.PostalCode
        );

        foreach (var itemDto in request.Items)
        {
            if (itemDto.PizzaId == Guid.Empty)
                throw new ArgumentException("PizzaId is required in each item.", nameof(itemDto.PizzaId));
            if (itemDto.Quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(itemDto.Quantity), "Quantity must be > 0.");

            // (Opcional) podrías validar que la pizza exista y tomar su nombre/precio real
            var pizza = await _pizzaRepository.GetByIdAsync(itemDto.PizzaId, ct);
            if (pizza is null)
                throw new InvalidOperationException($"Pizza {itemDto.PizzaId} does not exist.");

            var orderItem = new OrderItem(
                itemDto.PizzaId,
                pizza.Name,          // usamos el nombre real
                itemDto.Quantity,
                pizza.BasePrice      // o un precio enviado por el request si tu modelo lo pide
            );

            order.AddItem(orderItem);
        }

        await _orderRepository.AddAsync(order, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return MapToDto(order);
    }

    // ------------------ Cambiar estado ------------------

    public async Task<OrderDto?> ChangeStatusAsync(UpdateOrderStatusRequest request, CancellationToken ct = default)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id, ct);
        if (order is null)
            return null;

        // Aquí decides cómo traduces "NewStatus" a métodos de dominio
        switch (request.NewStatus)
        {
            case "Confirmed":
                order.Confirm();
                break;
            case "InKitchen":
                order.StartKitchen();
                break;
            case "OutForDelivery":
                order.StartDelivery();
                break;
            case "Delivered":
                order.Deliver();
                break;
            case "Cancelled":
                order.Cancel(request.Reason ?? "Cancelled from application layer.");
                break;
            default:
                throw new ArgumentException($"Unknown status: {request.NewStatus}", nameof(request.NewStatus));
        }

        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(ct);

        return MapToDto(order);
    }

    // ------------------ Eliminar (opcional) ------------------

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, ct);
        if (order is null)
            return false;

        _orderRepository.Remove(order);
        await _unitOfWork.SaveChangesAsync(ct);

        return true;
    }

    // ------------------ Mapping entidad -> DTO ------------------

    private static OrderDto MapToDto(Order entity)
    {
        return new OrderDto
        {
            Id = entity.Id,
            CustomerId = entity.CustomerId,
            Street = entity.Street,
            City = entity.City,
            PostalCode = entity.PostalCode,
            Status = entity.Status,
            StatusReason = entity.StatusReason,
            Total = entity.Total,
            Items = entity.Items
                .Select(i => new OrderItemDto
                {
                    PizzaId = i.PizzaId,
                    PizzaName = i.PizzaName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    LineTotal = i.LineTotal
                })
                .ToList()
        };
    }
}
