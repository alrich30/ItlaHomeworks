using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PizzaDeliverySystem.Domain.Entities;

namespace PizzaDeliverySystem.Domain.Core.Repository
{
    public interface IUserRepository
    {
        // Operaciones básicas
        Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);

        // Consultas
        Task<List<User>> GetAllActiveAsync(CancellationToken ct = default);
        Task<List<User>> GetAllAsync(CancellationToken ct = default); // Para admin - incluye inactivos

        // Operaciones de escritura
        Task AddAsync(User user, CancellationToken ct = default);
        void Update(User user);

        // NO hay Remove() - solo soft delete mediante Update()
    }
}

