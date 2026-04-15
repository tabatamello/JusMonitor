using JusMonitor.Application.Common.Interfaces;
using JusMonitor.Domain.Entities;
using MediatR;

namespace JusMonitor.Application.UseCases.Processos.CadastrarProcesso;

public class CadastrarProcessoHandler(
    IProcessoRepository processoRepository,
    IAdvogadoRepository advogadoRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CadastrarProcessoCommand, CadastrarProcessoResult>
{
    public async Task<CadastrarProcessoResult> Handle(
        CadastrarProcessoCommand request,
        CancellationToken cancellationToken)
    {
        var advogado = await advogadoRepository.ObterPorIdAsync(request.AdvogadoId, cancellationToken)
            ?? throw new InvalidOperationException("Advogado não encontrado.");

        if (!advogado.OABValidada)
            throw new InvalidOperationException("Advogado com OAB pendente de validação.");

        var jaExiste = await processoRepository.ObterPorNumeroAsync(request.NumeroProcesso, cancellationToken);
        if (jaExiste is not null)
            throw new InvalidOperationException("Processo já cadastrado.");

        var processo = Processo.Criar(
            request.NumeroProcesso,
            request.Tribunal,
            request.AdvogadoId,
            request.ClienteId,
            request.Vara);

        await processoRepository.AdicionarAsync(processo, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new CadastrarProcessoResult(processo.Id, processo.NumeroProcesso, processo.Tribunal);
    }
}