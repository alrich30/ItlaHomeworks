using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaDeliverySystem.Infrastructure.Models
{
    public class PizzaModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Size { get; set; } = "Medium";
        public decimal BasePrice { get; set; }

        public ICollection<IngredientModel> Ingredients { get; set; } = new List<IngredientModel>();
    }
}
