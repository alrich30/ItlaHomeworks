using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PizzaDeliverySystem.Domain.Core.Repository;
using PizzaDeliverySystem.Domain.Entities;

namespace PizzaDeliverySystem.Domain.Core.Repository;

public interface IOrderRepository : IRepository<Order>
{
    // Ejemplo: obtener órdenes de un cliente
    // Task<IReadOnlyList<Order>> GetByCustomerAsync(Guid customerId, CancellationToken ct = default);

    // Ejemplo: obtener órdenes por estado
    // Task<IReadOnlyList<Order>> GetByStatusAsync(string status, CancellationToken ct = default);
}
