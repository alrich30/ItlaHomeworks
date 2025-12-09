namespace PizzaDeliverySystem.Application.Dtos.Pizza;

public class UpdatePizzaRequest
{
    //public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }

    // Lista de ingredientes que debe tener la pizza
    public List<Guid> IngredientIds { get; set; } = new();
}
