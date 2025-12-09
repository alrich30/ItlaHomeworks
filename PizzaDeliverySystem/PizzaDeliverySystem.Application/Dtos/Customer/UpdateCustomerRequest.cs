namespace PizzaDeliverySystem.Application.Dtos.Customer;

public class UpdateCustomerRequest : CreateCustomerRequest
{
    public Guid Id { get; set; }
}
