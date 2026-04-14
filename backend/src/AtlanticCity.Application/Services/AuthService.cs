using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using AtlanticCity.Application.DTOs;
using AtlanticCity.Application.Interfaces;
using AtlanticCity.Domain.Entities;
using AtlanticCity.Domain.Interfaces;

namespace AtlanticCity.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _jwtSecret;
    private readonly int _jwtExpirationMinutes;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _jwtSecret = configuration["Jwt:Secret"] ?? throw new ArgumentNullException("Jwt:Secret no configurado");
        var expiration = configuration["Jwt:ExpirationMinutes"] ?? "60";
        _jwtExpirationMinutes = int.Parse(expiration);
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var usuario = await _unitOfWork.Usuarios.GetByEmailAsync(request.Email);
        
        if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
        {
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        if (!usuario.Activo)
        {
            throw new UnauthorizedAccessException("Usuario inactivo");
        }

        var token = GenerateJwtToken(usuario);

        return new LoginResponseDto
        {
            Token = token,
            ExpiresIn = _jwtExpirationMinutes * 60
        };
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Rol.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: "AtlanticCity",
            audience: "AtlanticCity",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}