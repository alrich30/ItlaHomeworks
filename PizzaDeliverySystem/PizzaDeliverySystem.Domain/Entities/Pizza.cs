using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzaDeliverySystem.Domain.Core;
using PizzaDeliverySystem.Domain.Core.Errors;

namespace PizzaDeliverySystem.Domain.Entities
{
    public class Pizza : BaseEntity, IAggregateRoot
    {
        private readonly List<Ingredient> _ingredients = new();

        public string Name { get; private set; } = string.Empty;
        public string Size { get; private set; } = string.Empty;
        public decimal BasePrice { get; private set; }

        public IReadOnlyCollection<Ingredient> Ingredients => _ingredients.AsReadOnly();

        private static readonly HashSet<string> AllowedSizes = new(StringComparer.OrdinalIgnoreCase)
    { "Small", "Medium", "Large" };


        // 👉 Constructor público: CREACIÓN de una nueva pizza (POST)
        public Pizza(string name, string size, decimal basePrice)
        {
            Id = Guid.NewGuid();         // aquí sí generamos el Guid             
            SetName(name);                   
            SetSize(size);
            SetBasePrice(basePrice);

        }

        // 👉 Constructor interno: HIDRATACIÓN desde la base de datos
        internal Pizza(Guid id, string name, string size, decimal basePrice)
        {
            Id = id;                            // usamos el Id que viene de la BD
            SetName(name);
            SetSize(size);
            SetBasePrice(basePrice);
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Name is required.");

            Name = name.Trim();
            Touch();
        }

        public void SetSize(string size)
        {
            if (string.IsNullOrWhiteSpace(size))
                throw new DomainException("Size is required.");

            if (!AllowedSizes.Contains(size))
                throw new DomainException("Invalid size. Allowed: Small, Medium, Large.");

            // Normalizamos a formato ‘Title case’ básico
            Size = char.ToUpper(size[0]) + size.Substring(1).ToLowerInvariant();
            Touch();
        }

        public void SetBasePrice(decimal basePrice)
        {
            if (basePrice < 0m)
                throw new DomainException("Base price cannot be negative.");

            BasePrice = decimal.Round(basePrice, 2);
            Touch();
        }

        public void AddIngredient(Ingredient ingredient)
        {
            if (ingredient is null)
                throw new DomainException("Ingredient is required.");

            if (_ingredients.Any(i => i.Id == ingredient.Id))
                throw new DomainException("Ingredient already added.");

            _ingredients.Add(ingredient);
            Touch();
        }

        public void RemoveIngredient(Guid ingredientId)
        {
            var removed = _ingredients.RemoveAll(i => i.Id == ingredientId);
            if (removed == 0)
                throw new DomainException("Ingredient not found.");

            Touch();
        }

        public decimal GetTotalPrice()
            => BasePrice + _ingredients.Sum(i => i.ExtraPrice);
    }

}
