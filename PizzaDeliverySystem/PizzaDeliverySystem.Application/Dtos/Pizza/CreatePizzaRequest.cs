namespace PizzaDeliverySystem.Application.Dtos.Pizza;

public class CreatePizzaRequest
{
    public string Name { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public List<IngredientDto> Ingredients { get; set; } = new();
}
