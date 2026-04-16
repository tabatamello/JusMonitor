using JusMonitor.Application.Common.Interfaces;
using JusMonitor.Application.UseCases.Movimentacoes.VerificarMovimentacoes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

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
        var numeroLimpo = numeroProcesso.Replace("-", "").Replace(".", "");

        var json = JsonSerializer.Serialize(new
        {
            query = new { match = new { numeroProcesso = numeroLimpo } }
        });

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var httpClient = HttpContext.RequestServices
            .GetRequiredService<IHttpClientFactory>()
            .CreateClient();

        httpClient.DefaultRequestHeaders.Add("Authorization",
            $"ApiKey {HttpContext.RequestServices.GetRequiredService<IConfiguration>()["DataJud:ApiKey"]}");

        var response = await httpClient.PostAsync(
            "https://api-publica.datajud.cnj.jus.br/api_publica_tjsp/_search", content, ct);

        var raw = await response.Content.ReadAsStringAsync(ct);

        return Ok(new { status = response.StatusCode, body = raw });
    }
}