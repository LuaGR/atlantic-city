using Moq;
using AtlanticCity.Application.DTOs;
using AtlanticCity.Application.Services;
using AtlanticCity.Domain.Entities;
using AtlanticCity.Domain.Interfaces;

namespace AtlanticCity.Tests.Services;

public class PedidoServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPedidoRepository> _mockPedidoRepo;
    private readonly PedidoService _service;

    public PedidoServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPedidoRepo = new Mock<IPedidoRepository>();
        _mockUnitOfWork.Setup(u => u.Pedidos).Returns(_mockPedidoRepo.Object);
        _service = new PedidoService(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsPedidos()
    {
        var pedidos = new List<Pedido>
        {
            new() { Id = 1, NumeroPedido = "PED-001", Cliente = "Cliente 1", Total = 100, Estado = EstadoPedido.Registrado },
            new() { Id = 2, NumeroPedido = "PED-002", Cliente = "Cliente 2", Total = 200, Estado = EstadoPedido.Procesado }
        };
        
        _mockPedidoRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(pedidos);

        var result = await _service.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsPedido()
    {
        var pedido = new Pedido { Id = 1, NumeroPedido = "PED-001", Cliente = "Test", Total = 100, Estado = EstadoPedido.Registrado };
        
        _mockPedidoRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(pedido);

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("PED-001", result.NumeroPedido);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        _mockPedidoRepo.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((Pedido?)null);

        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsCreatedPedido()
    {
        var dto = new CreatePedidoDto
        {
            NumeroPedido = "PED-NEW",
            Cliente = "Nuevo Cliente",
            Fecha = DateTime.UtcNow,
            Total = 150,
            Estado = EstadoPedidoDto.Registrado
        };
        
        _mockPedidoRepo.Setup(r => r.GetByNumeroPedidoAsync("PED-NEW", It.IsAny<CancellationToken>())).ReturnsAsync((Pedido?)null);
        _mockPedidoRepo.Setup(r => r.AddAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>())).ReturnsAsync((Pedido p, CancellationToken _) => p);

        var result = await _service.CreateAsync(dto);

        Assert.Equal("PED-NEW", result.NumeroPedido);
        Assert.Equal(150, result.Total);
    }

    [Fact]
    public async Task CreateAsync_DuplicateNumeroPedido_Throws()
    {
        var dto = new CreatePedidoDto
        {
            NumeroPedido = "PED-DUPLICADO",
            Cliente = "Test",
            Fecha = DateTime.UtcNow,
            Total = 100
        };
        var existing = new Pedido { Id = 1, NumeroPedido = "PED-DUPLICADO" };
        
        _mockPedidoRepo.Setup(r => r.GetByNumeroPedidoAsync("PED-DUPLICADO", It.IsAny<CancellationToken>())).ReturnsAsync(existing);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task UpdateAsync_ExistingId_UpdatesPedido()
    {
        var dto = new UpdatePedidoDto
        {
            Cliente = "Cliente Actualizado",
            Fecha = DateTime.UtcNow,
            Total = 200,
            Estado = EstadoPedidoDto.Procesado
        };
        var existing = new Pedido { Id = 1, NumeroPedido = "PED-001", Cliente = "Original", Total = 100, Estado = EstadoPedido.Registrado };
        
        _mockPedidoRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existing);
        _mockPedidoRepo.Setup(r => r.UpdateAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var result = await _service.UpdateAsync(1, dto);

        Assert.Equal("Cliente Actualizado", result.Cliente);
        Assert.Equal(200, result.Total);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_Throws()
    {
        var dto = new UpdatePedidoDto { Cliente = "Test", Total = 100 };
        
        _mockPedidoRepo.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((Pedido?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(999, dto));
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_SoftDeletes()
    {
        var existing = new Pedido { Id = 1, NumeroPedido = "PED-001", Eliminado = false };
        
        _mockPedidoRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existing);
        _mockPedidoRepo.Setup(r => r.UpdateAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _service.DeleteAsync(1);

        _mockPedidoRepo.Verify(r => r.UpdateAsync(It.Is<Pedido>(p => p.Eliminado == true), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingId_Throws()
    {
        _mockPedidoRepo.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((Pedido?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(999));
    }
}