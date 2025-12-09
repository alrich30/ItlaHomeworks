namespace PizzaDeliverySystem.Application.Dtos.Order;

public class OrderDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }

    public string Status { get; set; } = string.Empty;

    // Flattened delivery address
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;

    public decimal Total { get; set; }

    public List<OrderItemDto> Items { get; set; } = new();
}
