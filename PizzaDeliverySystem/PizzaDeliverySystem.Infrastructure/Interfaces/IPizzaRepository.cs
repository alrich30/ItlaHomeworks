using PizzaDeliverySystem.Domain.Core.Repository;
using PizzaDeliverySystem.Domain.Entities;

namespace PizzaDeliverySystem.Infrastructure.Interfaces;

public interface IPizzaRepository : IRepository<Pizza>
{
    // Aquí podrías añadir métodos específicos, por ejemplo:
    // Task<IReadOnlyList<Pizza>> GetBySizeAsync(string size, CancellationToken ct = default);
}
