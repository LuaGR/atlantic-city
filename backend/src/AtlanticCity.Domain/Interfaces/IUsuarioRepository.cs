using AtlanticCity.Domain.Entities;

namespace AtlanticCity.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Usuario> AddAsync(Usuario usuario, CancellationToken cancellationToken = default);
}