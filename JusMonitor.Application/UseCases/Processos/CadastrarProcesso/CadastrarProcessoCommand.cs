using MediatR;

namespace JusMonitor.Application.UseCases.Processos.CadastrarProcesso;

public record CadastrarProcessoCommand(
    string NumeroProcesso,
    string Tribunal,
    Guid AdvogadoId,
    Guid ClienteId,
    string? Vara = null
) : IRequest<CadastrarProcessoResult>;

public record CadastrarProcessoResult(Guid Id, string NumeroProcesso, string Tribunal);