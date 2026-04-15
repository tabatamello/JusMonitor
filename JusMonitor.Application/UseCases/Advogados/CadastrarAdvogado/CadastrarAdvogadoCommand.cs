using MediatR;

namespace JusMonitor.Application.UseCases.Advogados.CadastrarAdvogado;

public record CadastrarAdvogadoCommand(
    string Nome,
    string Email,
    string Senha,
    string NumeroOAB,
    string UfOAB
) : IRequest<CadastrarAdvogadoResult>;

public record CadastrarAdvogadoResult(Guid Id, string Nome, string Email, string NumeroOAB);