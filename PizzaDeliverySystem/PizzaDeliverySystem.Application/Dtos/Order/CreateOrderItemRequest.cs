namespace PizzaDeliverySystem.Application.Dtos.Order;

public class CreateOrderItemRequest
{
    public Guid PizzaId { get; set; }
    public string PizzaName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
