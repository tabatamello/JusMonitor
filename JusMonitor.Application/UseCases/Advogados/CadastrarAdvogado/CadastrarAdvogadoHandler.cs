using JusMonitor.Application.Common.Interfaces;
using JusMonitor.Domain.Entities;
using MediatR;

namespace JusMonitor.Application.UseCases.Advogados.CadastrarAdvogado;

public class CadastrarAdvogadoHandler(
    IAdvogadoRepository advogadoRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CadastrarAdvogadoCommand, CadastrarAdvogadoResult>
{
    public async Task<CadastrarAdvogadoResult> Handle(
        CadastrarAdvogadoCommand request,
        CancellationToken cancellationToken)
    {
        var emailEmUso = await advogadoRepository.ExisteEmailAsync(request.Email, cancellationToken);
        if (emailEmUso)
            throw new InvalidOperationException("E-mail já cadastrado.");

        var jaExisteOAB = await advogadoRepository.ObterPorOABAsync(request.NumeroOAB, request.UfOAB, cancellationToken);
        if (jaExisteOAB is not null)
            throw new InvalidOperationException("OAB já cadastrada.");

        var senhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha);

        var advogado = Advogado.Criar(request.Nome, request.Email, request.NumeroOAB, request.UfOAB, senhaHash);

        await advogadoRepository.AdicionarAsync(advogado, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new CadastrarAdvogadoResult(advogado.Id, advogado.Nome, advogado.Email, advogado.NumeroOAB);
    }
}