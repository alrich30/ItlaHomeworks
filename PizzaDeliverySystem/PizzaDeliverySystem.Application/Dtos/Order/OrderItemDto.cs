namespace PizzaDeliverySystem.Application.Dtos.Order;

public class OrderItemDto
{
    public Guid PizzaId { get; set; }
    public string PizzaName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}
