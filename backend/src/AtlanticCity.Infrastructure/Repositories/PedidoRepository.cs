using AtlanticCity.Domain.Entities;
using AtlanticCity.Domain.Interfaces;
using AtlanticCity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AtlanticCity.Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly AtlanticCityDbContext _context;

    public PedidoRepository(AtlanticCityDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pedido>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Pedidos.ToListAsync(cancellationToken);
    }

    public async Task<Pedido?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Pedidos.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Pedido?> GetByNumeroPedidoAsync(string numeroPedido, CancellationToken cancellationToken = default)
    {
        return await _context.Pedidos.FirstOrDefaultAsync(p => p.NumeroPedido == numeroPedido, cancellationToken);
    }

    public async Task<Pedido> AddAsync(Pedido pedido, CancellationToken cancellationToken = default)
    {
        _context.Pedidos.Add(pedido);
        return pedido;
    }

    public async Task UpdateAsync(Pedido pedido, CancellationToken cancellationToken = default)
    {
        _context.Pedidos.Update(pedido);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var pedido = await GetByIdAsync(id, cancellationToken);
        if (pedido != null)
        {
            pedido.Eliminado = true;
            _context.Pedidos.Update(pedido);
        }
    }
}