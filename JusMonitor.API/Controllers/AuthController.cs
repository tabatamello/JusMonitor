using JusMonitor.API.Services;
using JusMonitor.Application.Common.Interfaces;
using JusMonitor.Application.UseCases.Advogados.CadastrarAdvogado;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JusMonitor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator, IAdvogadoRepository advogadoRepository, TokenService tokenService) : ControllerBase
{
    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] CadastrarAdvogadoCommand command, CancellationToken ct)
    {
        try
        {
            var result = await mediator.Send(command, ct);
            return Created($"/api/advogados/{result.Id}", result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var advogado = await advogadoRepository.ObterPorEmailAsync(request.Email, ct);

        if (advogado is null || !BCrypt.Net.BCrypt.Verify(request.Senha, advogado.SenhaHash))
            return Unauthorized(new { erro = "E-mail ou senha inválidos." });

        var token = tokenService.GerarToken(advogado);

        return Ok(new
        {
            token,
            advogado = new
            {
                advogado.Id,
                advogado.Nome,
                advogado.Email,
                advogado.NumeroOAB,
                advogado.OABValidada
            }
        });
    }
}

public record LoginRequest(string Email, string Senha);