using AtlanticCity.Domain.Entities;
using AtlanticCity.Domain.Interfaces;
using AtlanticCity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AtlanticCity.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AtlanticCityDbContext _context;

    public UsuarioRepository(AtlanticCityDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<Usuario> AddAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        _context.Usuarios.Add(usuario);
        return usuario;
    }
}