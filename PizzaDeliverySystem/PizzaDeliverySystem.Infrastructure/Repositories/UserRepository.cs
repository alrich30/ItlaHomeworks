using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PizzaDeliverySystem.Domain.Core.Repository;
using PizzaDeliverySystem.Domain.Entities;
using PizzaDeliverySystem.Domain.Exceptions;
using PizzaDeliverySystem.Infrastructure.Context;
using PizzaDeliverySystem.Infrastructure.Models;

namespace PizzaDeliverySystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PizzaDbContext _context;
        private readonly DbSet<UserModel> _dbSet;

        public UserRepository(PizzaDbContext context)
        {
            _context = context;
            _dbSet = context.Set<UserModel>();
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            try
            {
                var model = await _dbSet.FindAsync(new object[] { id }, ct);
                return model != null ? MapToDomain(model) : null;
            }
            catch (Exception ex)
            {
                throw new UserRepositoryException($"Error retrieving user with ID '{id}'.", ex);
            }
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            try
            {
                var model = await _dbSet
                    .FirstOrDefaultAsync(u => u.Email == email && u.IsActive, ct);

                return model != null ? MapToDomain(model) : null;
            }
            catch (Exception ex)
            {
                throw new UserRepositoryException($"Error retrieving user with email '{email}'.", ex);
            }

        }
        public async Task<List<User>> GetAllActiveAsync(CancellationToken ct = default)
        {
            try
            {
                var models = await _dbSet
                    .Where(u => u.IsActive)
                    .AsNoTracking()
                    .ToListAsync(ct);

                return models.Select(MapToDomain).ToList();
            }
            catch (Exception ex)
            {
                throw new UserRepositoryException("Error retrieving active users.", ex);
            }

        }

        public async Task<List<User>> GetAllAsync(CancellationToken ct = default)
        {
            try
            {
                // Incluye activos e inactivos (solo para admin)
                var models = await _dbSet
                    .AsNoTracking()
                    .ToListAsync(ct);

                return models.Select(MapToDomain).ToList();
            }
            catch (Exception ex)
            {
                throw new UserRepositoryException("Error retrieving all users.", ex);
            }
        }

        public async Task AddAsync(User user, CancellationToken ct = default)
        {
            try
            {
                var model = MapToModel(user);
                await _dbSet.AddAsync(model, ct);
            }
            catch (Exception ex)
            {
                throw new UserRepositoryException($"Error adding user with email '{user.Email}'.", ex);
            }

        }

        public void Update(User user)
        {
            try
            {
                var model = MapToModel(user);
                _dbSet.Update(model);
            }
            catch (Exception ex)
            {
                throw new UserRepositoryException($"Error updating user with ID '{user.Id}'.", ex);
            }

        }

        // NO hay Remove() físico - solo soft delete
        public void Deactivate(User user)
        {
            user.Deactivate(); // Método de la entidad de dominio
            Update(user);
        }

        public void Reactivate(User user)
        {
            user.Reactivate();
            Update(user);
        }

        // Mapeo: UserModel → User (Domain)
        private User MapToDomain(UserModel model)
        {
            try
            {
                return new User(
                    model.Id,
                    model.Email,
                    model.PasswordHash,
                    model.Role,
                    model.IsActive
                );
            }
            catch (Exception ex)
            {
                throw new UserRepositoryException($"Error mapping UserModel to User entity for ID '{model.Id}'.", ex);
            }
        }

        // Mapeo: User (Domain) → UserModel
        private UserModel MapToModel(User user)
        {
            try
            {
                return new UserModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    Role = user.Role,
                    IsActive = user.IsActive
                };
            }
            catch (Exception ex)
            {
                throw new UserRepositoryException($"Error mapping User entity to UserModel for ID '{user.Id}'.", ex);
            }
        }
    }
}






  
