using AtlanticCity.Domain.Interfaces;
using AtlanticCity.Infrastructure.Persistence;
using AtlanticCity.Infrastructure.Repositories;

namespace AtlanticCity.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AtlanticCityDbContext _context;
    private PedidoRepository? _pedidoRepository;
    private UsuarioRepository? _usuarioRepository;

    public UnitOfWork(AtlanticCityDbContext context)
    {
        _context = context;
    }

    public IPedidoRepository Pedidos => _pedidoRepository ??= new PedidoRepository(_context);
    public IUsuarioRepository Usuarios => _usuarioRepository ??= new UsuarioRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}