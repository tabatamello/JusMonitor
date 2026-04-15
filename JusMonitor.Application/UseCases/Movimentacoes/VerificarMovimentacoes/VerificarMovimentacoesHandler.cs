using JusMonitor.Application.Common.Interfaces;
using JusMonitor.Domain.Entities;
using MediatR;

namespace JusMonitor.Application.UseCases.Movimentacoes.VerificarMovimentacoes;

public class VerificarMovimentacoesHandler(
    IProcessoRepository processoRepository,
    IMovimentacaoRepository movimentacaoRepository,
    IDataJudService dataJudService,
    IUnitOfWork unitOfWork
) : IRequestHandler<VerificarMovimentacoesCommand, VerificarMovimentacoesResult>
{
    public async Task<VerificarMovimentacoesResult> Handle(
        VerificarMovimentacoesCommand request,
        CancellationToken cancellationToken)
    {
        var processo = await processoRepository.ObterPorIdAsync(request.ProcessoId, cancellationToken)
            ?? throw new InvalidOperationException("Processo não encontrado.");

        var resultado = await dataJudService.ConsultarProcessoAsync(processo.NumeroProcesso, cancellationToken);

        if (resultado is null)
        {
            processo.RegistrarConsulta();
            await unitOfWork.CommitAsync(cancellationToken);
            return new VerificarMovimentacoesResult(0);
        }

        var novas = 0;

        foreach (var mov in resultado.Movimentacoes)
        {
            var jaExiste = await movimentacaoRepository.ExisteAsync(
                mov.Descricao, mov.DataMovimentacao, processo.Id, cancellationToken);

            if (jaExiste) continue;

            var movimentacao = Movimentacao.Criar(
                mov.Descricao,
                mov.DataMovimentacao,
                processo.Id,
                mov.CodigoMovimento);

            processo.AdicionarMovimentacao(movimentacao);
            await movimentacaoRepository.AdicionarAsync(movimentacao, cancellationToken);
            novas++;
        }

        await unitOfWork.CommitAsync(cancellationToken);

        return new VerificarMovimentacoesResult(novas);
    }
}