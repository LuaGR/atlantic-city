namespace AtlanticCity.Application.DTOs;

public enum EstadoPedidoDto
{
    Registrado = 1,
    Procesado = 2,
    Enviado = 3,
    Entregado = 4,
    Cancelado = 5
}

public class PedidoDto
{
    public int Id { get; set; }
    public string NumeroPedido { get; set; } = string.Empty;
    public string Cliente { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public EstadoPedidoDto Estado { get; set; }
}

public class CreatePedidoDto
{
    public string NumeroPedido { get; set; } = string.Empty;
    public string Cliente { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public EstadoPedidoDto Estado { get; set; } = EstadoPedidoDto.Registrado;
}

public class UpdatePedidoDto
{
    public string Cliente { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public EstadoPedidoDto Estado { get; set; }
}