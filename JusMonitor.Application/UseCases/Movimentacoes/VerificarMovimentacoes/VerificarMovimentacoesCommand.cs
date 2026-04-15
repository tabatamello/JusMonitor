using MediatR;

namespace JusMonitor.Application.UseCases.Movimentacoes.VerificarMovimentacoes;

public record VerificarMovimentacoesCommand(Guid ProcessoId) : IRequest<VerificarMovimentacoesResult>;

public record VerificarMovimentacoesResult(int NovasMovimentacoes);