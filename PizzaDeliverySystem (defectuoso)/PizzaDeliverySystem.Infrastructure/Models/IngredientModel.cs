using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaDeliverySystem.Infrastructure.Models
{
    public class IngredientModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal ExtraPrice { get; set; }

        // Relación muchos-a-muchos con Pizza
        public ICollection<PizzaModel> Pizzas { get; set; } = new List<PizzaModel>();
    }
}
