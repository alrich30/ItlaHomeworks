using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaDeliverySystem.Infrastructure.Models
{
    public class OrderItemModel
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public OrderModel Order { get; set; } = null!;

        public Guid PizzaId { get; set; }
        public string PizzaName { get; set; } = string.Empty;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
