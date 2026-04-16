using MediatR;

namespace JusMonitor.Application.UseCases.Clientes.CadastrarCliente;

public record CadastrarClienteCommand(
    string Nome,
    string Email,
    string? Telefone = null
) : IRequest<CadastrarClienteResult>;

public record CadastrarClienteResult(Guid Id, string Nome, string Email);