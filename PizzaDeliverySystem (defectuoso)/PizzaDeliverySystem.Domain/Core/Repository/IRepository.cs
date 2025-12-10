using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzaDeliverySystem.Domain.Core;

namespace PizzaDeliverySystem.Domain.Core.Repository
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        Task<List<T>> GetAllAsync(CancellationToken ct = default);
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(T entity, CancellationToken ct = default);
        void Update(T entity);
        void Remove(T entity);
    }
}
