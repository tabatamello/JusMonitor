using JusMonitor.Application.Common.Interfaces;
using JusMonitor.Application.UseCases.Clientes.CadastrarCliente;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JusMonitor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientesController(IMediator mediator, IClienteRepository clienteRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarClienteCommand command, CancellationToken ct)
    {
        try
        {
            var result = await mediator.Send(command, ct);
            return Created($"/api/clientes/{result.Id}", result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken ct)
    {
        var cliente = await clienteRepository.ObterPorIdAsync(id, ct);
        if (cliente is null) return NotFound();

        return Ok(new
        {
            cliente.Id,
            cliente.Nome,
            cliente.Email,
            cliente.Telefone
        });
    }
}