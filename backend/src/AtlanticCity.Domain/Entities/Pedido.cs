namespace AtlanticCity.Domain.Entities;

public class Pedido
{
    public int Id { get; set; }
    public string NumeroPedido { get; set; } = string.Empty;
    public string Cliente { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public EstadoPedido Estado { get; set; } = EstadoPedido.Registrado;
    public bool Eliminado { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public enum EstadoPedido
{
    Registrado = 1,
    Procesado = 2,
    Enviado = 3,
    Entregado = 4,
    Cancelado = 5
}