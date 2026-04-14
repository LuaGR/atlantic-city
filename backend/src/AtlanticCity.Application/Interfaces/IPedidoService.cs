using AtlanticCity.Application.DTOs;

namespace AtlanticCity.Application.Interfaces;

public interface IPedidoService
{
    Task<IEnumerable<PedidoDto>> GetAllAsync();
    Task<PedidoDto?> GetByIdAsync(int id);
    Task<PedidoDto> CreateAsync(CreatePedidoDto dto);
    Task<PedidoDto> UpdateAsync(int id, UpdatePedidoDto dto);
    Task DeleteAsync(int id);
}