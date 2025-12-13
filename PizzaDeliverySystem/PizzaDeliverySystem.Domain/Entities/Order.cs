using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzaDeliverySystem.Domain.Core;
using PizzaDeliverySystem.Domain.Core.Errors;

namespace PizzaDeliverySystem.Domain.Entities
{
    public class Order : BaseEntity, IAggregateRoot
    {
        private readonly List<OrderItem> _items = new();

        public Guid CustomerId { get; private set; }
        public string Status { get; private set; } = "Created";
        public string Street { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string PostalCode { get; private set; } = string.Empty;

        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        public decimal Total => _items.Sum(it => it.LineTotal);

        private static readonly HashSet<string> AllowedStatuses = new(StringComparer.OrdinalIgnoreCase)
    { "Created", "Confirmed", "InKitchen", "OutForDelivery", "Delivered", "Cancelled" };

        public Order(Guid customerId, string street, string city, string postalCode)
        {
            Id = Guid.NewGuid();
            SetCustomer(customerId);
            SetAddress(street, city, postalCode);
            // Status starts as "Created"
        }

        internal Order(Guid id, Guid customerId, string street, string city, string postalCode)
        {
            Id = id;
            SetCustomer(customerId);
            SetAddress(street, city, postalCode);
            // Status starts as "Created"
        }

        public void SetCustomer(Guid customerId)
        {
            if (customerId == Guid.Empty)
                throw new DomainException("Customer ID cannot be empty.");

            CustomerId = customerId;
            Touch();
        }

        public void SetAddress(string street, string city, string postalCode)
        {
            if (string.IsNullOrWhiteSpace(street) ||
                string.IsNullOrWhiteSpace(city) ||
                string.IsNullOrWhiteSpace(postalCode))
                throw new DomainException("Complete address is required.");

            Street = street.Trim();
            City = city.Trim();
            PostalCode = postalCode.Trim();
            Touch();
        }

        public void AddItem(OrderItem item)
        {
            if (!IsModifiable())
                throw new DomainException("Cannot modify items after preparation started.");

            if (item is null)
                throw new DomainException("Item is required.");

            _items.Add(item);
            Touch();
        }

        public void Confirm()
        {
            if (!_items.Any())
                throw new DomainException("Cannot confirm empty order.");

            EnsureStatus("Created");
            Status = "Confirmed";
            Touch();
        }

        public void StartKitchen()
        {
            EnsureStatus("Confirmed");
            Status = "InKitchen";
            Touch();
        }

        public void StartDelivery()
        {
            EnsureStatus("InKitchen");
            Status = "OutForDelivery";
            Touch();
        }

        public void Deliver()
        {
            EnsureStatus("OutForDelivery");
            Status = "Delivered";
            Touch();
        }

        public void Cancel(string reason)
        {
            if (Status.Equals("Delivered", StringComparison.OrdinalIgnoreCase))
                throw new DomainException("Delivered orders cannot be cancelled.");

            // (reason can be stored/logged later if needed)
            Status = "Cancelled";
            Touch();
        }

        private bool IsModifiable() =>
            Status.Equals("Created", StringComparison.OrdinalIgnoreCase) ||
            Status.Equals("Confirmed", StringComparison.OrdinalIgnoreCase);

        private void EnsureStatus(string expected)
        {
            if (!Status.Equals(expected, StringComparison.OrdinalIgnoreCase))
                throw new DomainException($"Invalid status transition from {Status} (expected {expected}).");
        }

        // Optional: generic setter if you necesitas validar un status externo
        public void SetStatus(string status)
        {
            if (!AllowedStatuses.Contains(status))
                throw new DomainException("Invalid status.");
            Status = status;
            Touch();
        }
    }
}
