using JusMonitor.Application.Common.Interfaces;
using JusMonitor.Application.UseCases.Processos.CadastrarProcesso;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JusMonitor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdvogadosController(IMediator mediator, IAdvogadoRepository advogadoRepository) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> Me(CancellationToken ct)
    {
        var id = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var advogado = await advogadoRepository.ObterPorIdAsync(id, ct);

        if (advogado is null) return NotFound();

        return Ok(new
        {
            advogado.Id,
            advogado.Nome,
            advogado.Email,
            advogado.NumeroOAB,
            advogado.UfOAB,
            advogado.OABValidada,
            advogado.DataValidacaoOAB
        });
    }

    [HttpPost("processos")]
    public async Task<IActionResult> CadastrarProcesso([FromBody] CadastrarProcessoRequest request, CancellationToken ct)
    {
        var advogadoId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        try
        {
            var command = new CadastrarProcessoCommand(
                request.NumeroProcesso,
                request.Tribunal,
                advogadoId,
                request.ClienteId,
                request.Vara);

            var result = await mediator.Send(command, ct);
            return Created($"/api/processos/{result.Id}", result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }
}

public record CadastrarProcessoRequest(
    string NumeroProcesso,
    string Tribunal,
    Guid ClienteId,
    string? Vara = null);