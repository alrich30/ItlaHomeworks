using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PizzaDeliverySystem.Domain.Core.Repository;
using PizzaDeliverySystem.Domain.Entities;

namespace PizzaDeliverySystem.Domain.Core.Repository;

public interface IPizzaRepository : IRepository<Pizza>
{
    // Ejemplo de método específico de pizzas:
    // Task<IReadOnlyList<Pizza>> GetBySizeAsync(string size, CancellationToken ct = default);
}
