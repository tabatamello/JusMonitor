using JusMonitor.Application.Common.Interfaces;
using JusMonitor.Application.UseCases.Movimentacoes.VerificarMovimentacoes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JusMonitor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProcessosController(
    IMediator mediator,
    IProcessoRepository processoRepository,
    IDataJudService dataJudService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken ct)
    {
        var advogadoId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var processos = await processoRepository.ListarPorAdvogadoAsync(advogadoId, ct);

        return Ok(processos.Select(p => new
        {
            p.Id,
            p.NumeroProcesso,
            p.Tribunal,
            p.Vara,
            p.Status,
            p.UltimaConsulta,
            TotalMovimentacoes = p.Movimentacoes.Count
        }));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken ct)
    {
        var processo = await processoRepository.ObterPorIdAsync(id, ct);
        if (processo is null) return NotFound();

        return Ok(new
        {
            processo.Id,
            processo.NumeroProcesso,
            processo.Tribunal,
            processo.Vara,
            processo.Assunto,
            processo.Status,
            processo.UltimaConsulta,
            Movimentacoes = processo.Movimentacoes.Select(m => new
            {
                m.Id,
                m.Descricao,
                m.DataMovimentacao,
                m.CodigoMovimento,
                m.NotificacaoEnviada
            })
        });
    }

    [HttpPost("{id:guid}/verificar")]
    public async Task<IActionResult> VerificarMovimentacoes(Guid id, CancellationToken ct)
    {
        try
        {
            var result = await mediator.Send(new VerificarMovimentacoesCommand(id), ct);
            return Ok(new { novasMovimentacoes = result.NovasMovimentacoes });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpGet("consultar-datajud")]
    public async Task<IActionResult> ConsultarDataJud([FromQuery] string numeroProcesso, CancellationToken ct)
    {
        var resultado = await dataJudService.ConsultarProcessoAsync(numeroProcesso, ct);

        if (resultado is null)
            return NotFound(new { erro = "Processo não encontrado no DataJud." });

        return Ok(resultado);
    }
}