using System;

namespace PizzaDeliverySystem.Application.Dtos;

public class UpdateOrderStatusRequest
{
    public Guid Id { get; set; }


    /// Nuevo estado de la orden. Ejemplos:
    /// "Confirmed", "InKitchen", "OutForDelivery", "Delivered", "Cancelled"

    public string NewStatus { get; set; } = string.Empty;


    /// Razón opcional para cancelación o transición especial.

    public string? Reason { get; set; }
}
