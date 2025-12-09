using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaDeliverySystem.Infrastructure.Models
{
    public class OrderModel
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }
        public CustomerModel Customer { get; set; } = null!;

        public string Status { get; set; } = "Created";

        public string? StatusReason { get; set; }

        // Dirección de entrega (podría ser distinta a la del cliente)
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;

        public ICollection<OrderItemModel> Items { get; set; } = new List<OrderItemModel>();
    }
}
