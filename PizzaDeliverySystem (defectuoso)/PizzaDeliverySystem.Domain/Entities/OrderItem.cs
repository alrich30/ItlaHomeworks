using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzaDeliverySystem.Domain.Core;
using PizzaDeliverySystem.Domain.Core.Errors;

namespace PizzaDeliverySystem.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public Guid PizzaId { get; private set; }
        public string PizzaName { get; private set; } = string.Empty;
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        public decimal LineTotal => UnitPrice * Quantity;

        public OrderItem(Guid pizzaId, string pizzaName, int quantity, decimal unitPrice)
        {
            Id = Guid.NewGuid();
            SetPizzaId(pizzaId);
            SetPizzaName(pizzaName);
            SetUnitPrice(unitPrice);
            ChangeQuantity(quantity);
        }
        internal OrderItem(Guid id, Guid pizzaId, string pizzaName, int quantity, decimal unitPrice)
        {
            Id = id;
            SetPizzaId(pizzaId);
            SetPizzaName(pizzaName);
            SetUnitPrice(unitPrice);
            ChangeQuantity(quantity);
        }


        private void SetPizzaId(Guid pizzaId)
        {
            if (pizzaId == Guid.Empty)
                throw new DomainException("Pizza ID cannot be empty.");

            PizzaId = pizzaId;
            Touch();
        }

        private void SetPizzaName(string pizzaName)
        {
            PizzaName = string.IsNullOrWhiteSpace(pizzaName) ? "Custom Pizza" : pizzaName.Trim();
            Touch();
        }

        private void SetUnitPrice(decimal unitPrice)
        {
            if (unitPrice < 0m)
                throw new DomainException("Unit price cannot be negative.");

            UnitPrice = decimal.Round(unitPrice, 2);
            Touch();
        }

        public void ChangeQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Quantity must be greater than zero.");

            Quantity = quantity;
            Touch();
        }
    }
}
