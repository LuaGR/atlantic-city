using AtlanticCity.Domain.Entities;

namespace AtlanticCity.Domain.Interfaces;

public interface IPedidoRepository
{
    Task<IEnumerable<Pedido>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Pedido?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Pedido?> GetByNumeroPedidoAsync(string numeroPedido, CancellationToken cancellationToken = default);
    Task<Pedido> AddAsync(Pedido pedido, CancellationToken cancellationToken = default);
    Task UpdateAsync(Pedido pedido, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}