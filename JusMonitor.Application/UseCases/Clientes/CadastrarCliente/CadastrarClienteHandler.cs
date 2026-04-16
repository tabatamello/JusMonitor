using JusMonitor.Application.Common.Interfaces;
using JusMonitor.Domain.Entities;
using MediatR;

namespace JusMonitor.Application.UseCases.Clientes.CadastrarCliente;

public class CadastrarClienteHandler(
    IClienteRepository clienteRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CadastrarClienteCommand, CadastrarClienteResult>
{
    public async Task<CadastrarClienteResult> Handle(
        CadastrarClienteCommand request,
        CancellationToken cancellationToken)
    {
        var emailEmUso = await clienteRepository.ExisteEmailAsync(request.Email, cancellationToken);
        if (emailEmUso)
            throw new InvalidOperationException("E-mail já cadastrado.");

        var cliente = Cliente.Criar(request.Nome, request.Email, request.Telefone);

        await clienteRepository.AdicionarAsync(cliente, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new CadastrarClienteResult(cliente.Id, cliente.Nome, cliente.Email);
    }
}