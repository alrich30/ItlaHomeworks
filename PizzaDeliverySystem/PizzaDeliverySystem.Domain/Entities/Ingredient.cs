using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzaDeliverySystem.Domain.Core;
using PizzaDeliverySystem.Domain.Core.Errors;

namespace PizzaDeliverySystem.Domain.Entities;

public class Ingredient : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public decimal ExtraPrice { get; private set; }

    public Ingredient(string name, decimal extraPrice)
    {
        Id = Guid.NewGuid();
        SetName(name);
        SetExtraPrice(extraPrice);
    }

    // Hidratar ingrediente existente desde la BD
    internal Ingredient(Guid id, string name, decimal extraPrice)
    {
        Id = id;
        Name = name;
        ExtraPrice = extraPrice;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name is required.");

        Name = name.Trim();
        Touch();
    }

    public void SetExtraPrice(decimal extraPrice)
    {
        if (extraPrice < 0m)
            throw new DomainException("Extra price cannot be negative.");

        ExtraPrice = decimal.Round(extraPrice, 2);
        Touch();
    }
}

