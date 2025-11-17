using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PizzaDeliverySystem.Application.Contract;
using PizzaDeliverySystem.Application.Dtos;
using PizzaDeliverySystem.Domain.Core.Repository;
using PizzaDeliverySystem.Domain.Entities;

namespace PizzaDeliverySystem.Application.Service;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    // ------------------ Consultar ------------------

    public async Task<CustomerDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var customer = await _customerRepository.GetByIdAsync(id, ct);
        return customer is null ? null : MapToDto(customer);
    }

    // ------------------ Crear ------------------

    public async Task<CustomerDto> CreateAsync(CreateCustomerRequest request, CancellationToken ct = default)
    {
        // Validaciones de aplicación (DTO)
        if (string.IsNullOrWhiteSpace(request.FullName))
            throw new ArgumentException("Full name is required.", nameof(request.FullName));

        if (string.IsNullOrWhiteSpace(request.Phone))
            throw new ArgumentException("Phone is required.", nameof(request.Phone));

        if (string.IsNullOrWhiteSpace(request.Street) ||
            string.IsNullOrWhiteSpace(request.City) ||
            string.IsNullOrWhiteSpace(request.PostalCode))
            throw new ArgumentException("Full delivery address is required.");

        // Construimos la entidad de dominio (las reglas de negocio se validan allí)
        var customer = new Customer(
            request.FullName,
            request.Phone,
            request.Street,
            request.City,
            request.PostalCode
        );

        await _customerRepository.AddAsync(customer, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return MapToDto(customer);
    }

    // ------------------ Actualizar ------------------

    public async Task<CustomerDto?> UpdateAsync(UpdateCustomerRequest request, CancellationToken ct = default)
    {
        var existing = await _customerRepository.GetByIdAsync(request.Id, ct);
        if (existing is null)
            return null;

        if (string.IsNullOrWhiteSpace(request.FullName))
            throw new ArgumentException("Full name is required.", nameof(request.FullName));

        if (string.IsNullOrWhiteSpace(request.Phone))
            throw new ArgumentException("Phone is required.", nameof(request.Phone));

        if (string.IsNullOrWhiteSpace(request.Street) ||
            string.IsNullOrWhiteSpace(request.City) ||
            string.IsNullOrWhiteSpace(request.PostalCode))
            throw new ArgumentException("Full delivery address is required.");

        // Usamos los métodos de dominio
        existing.SetFullName(request.FullName);
        existing.SetPhone(request.Phone);
        existing.SetAddress(request.Street, request.City, request.PostalCode);

        _customerRepository.Update(existing);
        await _unitOfWork.SaveChangesAsync(ct);

        return MapToDto(existing);
    }

    // ------------------ Eliminar ------------------

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var existing = await _customerRepository.GetByIdAsync(id, ct);
        if (existing is null)
            return false;

        _customerRepository.Remove(existing);
        await _unitOfWork.SaveChangesAsync(ct);

        return true;
    }

    // ------------------ Mapping entidad -> DTO ------------------

    private static CustomerDto MapToDto(Customer entity)
    {
        return new CustomerDto
        {
            Id = entity.Id,
            FullName = entity.FullName,
            Phone = entity.Phone,
            Street = entity.Street,
            City = entity.City,
            PostalCode = entity.PostalCode
        };
    }
}
