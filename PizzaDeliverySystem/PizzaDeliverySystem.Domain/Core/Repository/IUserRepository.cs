using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzaDeliverySystem.Domain.Entities;

namespace PizzaDeliverySystem.Domain.Core.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    }
}
