namespace PizzaDeliverySystem.Application.Dtos.Order;

public class CreateOrderRequest
{
    public Guid CustomerId { get; set; }

    // Delivery address
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;

    public List<CreateOrderItemRequest> Items { get; set; } = new();
}
