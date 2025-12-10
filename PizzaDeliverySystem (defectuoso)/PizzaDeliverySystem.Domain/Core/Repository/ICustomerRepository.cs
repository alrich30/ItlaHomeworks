using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PizzaDeliverySystem.Domain.Core.Repository;
using PizzaDeliverySystem.Domain.Entities;

namespace PizzaDeliverySystem.Domain.Core.Repository;

public interface ICustomerRepository : IRepository<Customer>
{
    // Ejemplo: buscar cliente por teléfono
    // Task<Customer?> GetByPhoneAsync(string phone, CancellationToken ct = default);
}
