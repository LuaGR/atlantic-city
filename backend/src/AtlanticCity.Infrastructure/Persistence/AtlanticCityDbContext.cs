using Microsoft.EntityFrameworkCore;
using AtlanticCity.Domain.Entities;

namespace AtlanticCity.Infrastructure.Persistence;

public class AtlanticCityDbContext : DbContext
{
    public AtlanticCityDbContext(DbContextOptions<AtlanticCityDbContext> options) : base(options) { }

    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.ToTable("Pedidos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NumeroPedido).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Cliente).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Total).HasPrecision(10, 2);
            entity.Property(e => e.Estado).HasConversion<string>();
            entity.HasIndex(e => e.NumeroPedido).IsUnique();
            entity.Ignore(e => e.Eliminado);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Rol).HasConversion<string>();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<Pedido>().HasQueryFilter(p => !p.Eliminado);
    }
}