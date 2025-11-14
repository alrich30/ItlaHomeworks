using PizzaDeliverySystem.Domain.Core.Repository;
using PizzaDeliverySystem.Domain.Entities;

namespace PizzaDeliverySystem.Infrastructure.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    // Ejemplo: obtener un pedido con sus ítems ya cargados
    // Task<Order?> GetWithItemsAsync(Guid id, CancellationToken ct = default);
}
