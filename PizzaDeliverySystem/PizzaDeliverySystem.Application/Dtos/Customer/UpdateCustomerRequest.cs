namespace PizzaDeliverySystem.Application.Dtos;

public class UpdateCustomerRequest : CreateCustomerRequest
{
    public Guid Id { get; set; }
}
