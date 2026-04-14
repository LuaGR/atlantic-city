using AtlanticCity.Domain.Entities;

namespace AtlanticCity.Domain.Interfaces;

public interface IUnitOfWork
{
    IPedidoRepository Pedidos { get; }
    IUsuarioRepository Usuarios { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}