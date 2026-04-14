using FluentValidation;
using AtlanticCity.Application.DTOs;
using AtlanticCity.Application.Validators;

namespace AtlanticCity.Tests.Validators;

public class CreatePedidoValidatorTests
{
    private readonly CreatePedidoValidator _validator;

    public CreatePedidoValidatorTests()
    {
        _validator = new CreatePedidoValidator();
    }

    [Fact]
    public void CreatePedidoValidator_ValidDto_Passes()
    {
        var dto = new CreatePedidoDto
        {
            NumeroPedido = "PED-001",
            Cliente = "Juan Perez",
            Fecha = DateTime.UtcNow,
            Total = 100.50m,
            Estado = EstadoPedidoDto.Registrado
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void CreatePedidoValidator_EmptyNumeroPedido_Fails()
    {
        var dto = new CreatePedidoDto
        {
            NumeroPedido = "",
            Cliente = "Juan Perez",
            Fecha = DateTime.UtcNow,
            Total = 100.50m
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "NumeroPedido");
    }

    [Fact]
    public void CreatePedidoValidator_ZeroTotal_Fails()
    {
        var dto = new CreatePedidoDto
        {
            NumeroPedido = "PED-001",
            Cliente = "Juan Perez",
            Fecha = DateTime.UtcNow,
            Total = 0
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Total");
    }

    [Fact]
    public void CreatePedidoValidator_NegativeTotal_Fails()
    {
        var dto = new CreatePedidoDto
        {
            NumeroPedido = "PED-001",
            Cliente = "Juan Perez",
            Fecha = DateTime.UtcNow,
            Total = -50
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Total");
    }

    [Fact]
    public void CreatePedidoValidator_EmptyCliente_Fails()
    {
        var dto = new CreatePedidoDto
        {
            NumeroPedido = "PED-001",
            Cliente = "",
            Fecha = DateTime.UtcNow,
            Total = 100
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Cliente");
    }

    [Fact]
    public void CreatePedidoValidator_MaxLengthNumeroPedido_Fails()
    {
        var dto = new CreatePedidoDto
        {
            NumeroPedido = new string('A', 51),
            Cliente = "Juan Perez",
            Fecha = DateTime.UtcNow,
            Total = 100
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "NumeroPedido");
    }

    [Fact]
    public void CreatePedidoValidator_MaxLengthCliente_Fails()
    {
        var dto = new CreatePedidoDto
        {
            NumeroPedido = "PED-001",
            Cliente = new string('A', 151),
            Fecha = DateTime.UtcNow,
            Total = 100
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Cliente");
    }
}