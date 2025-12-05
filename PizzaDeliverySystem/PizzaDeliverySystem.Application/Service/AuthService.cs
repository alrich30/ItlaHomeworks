using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PizzaDeliverySystem.Application.Contract;
using PizzaDeliverySystem.Application.Dtos.Auth;
using PizzaDeliverySystem.Domain.Core.Repository;
using PizzaDeliverySystem.Domain.Entities;

namespace PizzaDeliverySystem.Application.Service;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;

    public AuthService(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IConfiguration config)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _config = config;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var existing = await _userRepository.GetByEmailAsync(request.Email, ct);
        if (existing is not null)
            throw new InvalidOperationException("Email already registered.");

        var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User(request.Email, hash, "Customer");

        await _userRepository.AddAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        var token = GenerateJwt(user);

        return new AuthResponse
        {
            Token = token,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, ct);
        if (user is null)
            throw new InvalidOperationException("Invalid credentials.");

        var valid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!valid)
            throw new InvalidOperationException("Invalid credentials.");

        var token = GenerateJwt(user);

        return new AuthResponse
        {
            Token = token,
            Email = user.Email,
            Role = user.Role
        };
    }

    private string GenerateJwt(User user)
    {
        var key = _config["Jwt:Key"]!;
        var issuer = _config["Jwt:Issuer"]!;
        var audience = _config["Jwt:Audience"]!;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("uid", user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
