using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzaDeliverySystem.Domain.Core;
using PizzaDeliverySystem.Domain.Core.Errors;

namespace PizzaDeliverySystem.Domain.Entities
{
    public class Customer : BaseEntity, IAggregateRoot
    {
        public string FullName { get; private set; } = string.Empty;
        public string Phone { get; private set; } = string.Empty;

        // Reemplazo de Address (Value Object) por propiedades simples
        public string Street { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string PostalCode { get; private set; } = string.Empty;

        public Customer(string fullName, string phone, string street, string city, string postalCode)
        {
            SetFullName(fullName);
            SetPhone(phone);
            SetAddress(street, city, postalCode);
        }

        public void SetFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new DomainException("Full name is required.");

            FullName = fullName.Trim();
            Touch();
        }

        public void SetPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                throw new DomainException("Phone number is required.");

            // Validación sencilla del formato (solo dígitos y mínimo 8)
            if (phone.Length < 8 || !phone.All(char.IsDigit))
                throw new DomainException("Invalid phone number.");

            Phone = phone.Trim();
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
    }
}
