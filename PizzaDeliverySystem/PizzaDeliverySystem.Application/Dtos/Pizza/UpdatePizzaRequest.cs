namespace PizzaDeliverySystem.Application.Dtos;

public class UpdatePizzaRequest
{
    public string Name { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }

    // Lista de ingredientes que debe tener la pizza
    public List<Guid> IngredientIds { get; set; } = new();
}
