using AtlanticCity.Application.DTOs;
using AtlanticCity.Application.Interfaces;
using AtlanticCity.Domain.Entities;
using AtlanticCity.Domain.Interfaces;

namespace AtlanticCity.Application.Services;

public class PedidoService : IPedidoService
{
    private readonly IUnitOfWork _unitOfWork;

    public PedidoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PedidoDto>> GetAllAsync()
    {
        var pedidos = await _unitOfWork.Pedidos.GetAllAsync();
        return pedidos.Select(MapToDto);
    }

    public async Task<PedidoDto?> GetByIdAsync(int id)
    {
        var pedido = await _unitOfWork.Pedidos.GetByIdAsync(id);
        return pedido == null ? null : MapToDto(pedido);
    }

    public async Task<PedidoDto> CreateAsync(CreatePedidoDto dto)
    {
        var existente = await _unitOfWork.Pedidos.GetByNumeroPedidoAsync(dto.NumeroPedido);
        if (existente != null)
        {
            throw new InvalidOperationException($"El número de pedido '{dto.NumeroPedido}' ya existe");
        }

        var pedido = new Pedido
        {
            NumeroPedido = dto.NumeroPedido,
            Cliente = dto.Cliente,
            Fecha = dto.Fecha,
            Total = dto.Total,
            Estado = (EstadoPedido)dto.Estado,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Pedidos.AddAsync(pedido);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(pedido);
    }

    public async Task<PedidoDto> UpdateAsync(int id, UpdatePedidoDto dto)
    {
        var pedido = await _unitOfWork.Pedidos.GetByIdAsync(id);
        if (pedido == null)
        {
            throw new KeyNotFoundException($"Pedido con ID {id} no encontrado");
        }

        pedido.Cliente = dto.Cliente;
        pedido.Fecha = dto.Fecha;
        pedido.Total = dto.Total;
        pedido.Estado = (EstadoPedido)dto.Estado;
        pedido.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Pedidos.UpdateAsync(pedido);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(pedido);
    }

    public async Task DeleteAsync(int id)
    {
        var pedido = await _unitOfWork.Pedidos.GetByIdAsync(id);
        if (pedido == null)
        {
            throw new KeyNotFoundException($"Pedido con ID {id} no encontrado");
        }

        pedido.Eliminado = true;
        pedido.UpdatedAt = DateTime.UtcNow;
        
        await _unitOfWork.Pedidos.UpdateAsync(pedido);
        await _unitOfWork.SaveChangesAsync();
    }

    private static PedidoDto MapToDto(Pedido pedido)
    {
        return new PedidoDto
        {
            Id = pedido.Id,
            NumeroPedido = pedido.NumeroPedido,
            Cliente = pedido.Cliente,
            Fecha = pedido.Fecha,
            Total = pedido.Total,
            Estado = (EstadoPedidoDto)pedido.Estado
        };
    }
}
