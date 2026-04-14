using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AtlanticCity.Application.DTOs;
using AtlanticCity.Application.Interfaces;

namespace AtlanticCity.Api.Controllers;

[ApiController]
[Route("api/pedidos")]
[Authorize]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _pedidoService;
    public PedidosController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PedidoDto>>> GetAll()
    {
        var pedidos = await _pedidoService.GetAllAsync();
        return Ok(pedidos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PedidoDto>> GetById(int id)
    {
        var pedido = await _pedidoService.GetByIdAsync(id);
        if (pedido == null) return NotFound();
        return Ok(pedido);
    }

    [HttpPost]
    public async Task<ActionResult<PedidoDto>> Create([FromBody] CreatePedidoDto dto)
    {
        var created = await _pedidoService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PedidoDto>> Update(int id, [FromBody] UpdatePedidoDto dto)
    {
        var updated = await _pedidoService.UpdateAsync(id, dto);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _pedidoService.DeleteAsync(id);
        return NoContent();
    }
}
