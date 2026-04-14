namespace AtlanticCity.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public RolUsuario Rol { get; set; } = RolUsuario.User;
    public bool Activo { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}

public enum RolUsuario
{
    User = 1,
    Admin = 2
}