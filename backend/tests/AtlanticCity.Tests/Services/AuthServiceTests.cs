using Microsoft.Extensions.Configuration;
using Moq;
using AtlanticCity.Application.DTOs;
using AtlanticCity.Application.Services;
using AtlanticCity.Domain.Entities;
using AtlanticCity.Domain.Interfaces;

namespace AtlanticCity.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IUsuarioRepository> _mockUsuarioRepo;
    private readonly IConfiguration _configuration;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUsuarioRepo = new Mock<IUsuarioRepository>();
        _mockUnitOfWork.Setup(u => u.Usuarios).Returns(_mockUsuarioRepo.Object);

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Secret"] = "AtlanticCityTestSecretKey2026SuperSecure!",
                ["Jwt:ExpirationMinutes"] = "60"
            })
            .Build();

        _service = new AuthService(_mockUnitOfWork.Object, _configuration);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsToken()
    {
        var usuario = new Usuario
        {
            Id = 1,
            Email = "admin@atlanticcity.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
            Rol = RolUsuario.Admin,
            Activo = true
        };

        _mockUsuarioRepo
            .Setup(r => r.GetByEmailAsync("admin@atlanticcity.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);

        var result = await _service.LoginAsync(new LoginRequestDto
        {
            Email = "admin@atlanticcity.com",
            Password = "Password123!"
        });

        Assert.NotNull(result.Token);
        Assert.NotEmpty(result.Token);
        Assert.Equal(3600, result.ExpiresIn);
    }

    [Fact]
    public async Task LoginAsync_InvalidEmail_ThrowsUnauthorized()
    {
        _mockUsuarioRepo
            .Setup(r => r.GetByEmailAsync("noexiste@test.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Usuario?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _service.LoginAsync(new LoginRequestDto
            {
                Email = "noexiste@test.com",
                Password = "cualquiera"
            }));
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ThrowsUnauthorized()
    {
        var usuario = new Usuario
        {
            Id = 2,
            Email = "user@atlanticcity.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
            Rol = RolUsuario.User,
            Activo = true
        };

        _mockUsuarioRepo
            .Setup(r => r.GetByEmailAsync("user@atlanticcity.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _service.LoginAsync(new LoginRequestDto
            {
                Email = "user@atlanticcity.com",
                Password = "ContraseñaIncorrecta!"
            }));
    }

    [Fact]
    public async Task LoginAsync_InactiveUser_ThrowsUnauthorized()
    {
        var usuario = new Usuario
        {
            Id = 3,
            Email = "inactivo@atlanticcity.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
            Rol = RolUsuario.User,
            Activo = false
        };

        _mockUsuarioRepo
            .Setup(r => r.GetByEmailAsync("inactivo@atlanticcity.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _service.LoginAsync(new LoginRequestDto
            {
                Email = "inactivo@atlanticcity.com",
                Password = "Password123!"
            }));
    }
}
