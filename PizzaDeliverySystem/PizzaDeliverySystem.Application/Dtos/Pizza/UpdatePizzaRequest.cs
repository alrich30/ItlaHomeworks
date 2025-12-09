namespace PizzaDeliverySystem.Application.Dtos;

public class UpdatePizzaRequest : CreatePizzaRequest
{
    public Guid Id { get; set; }
}
