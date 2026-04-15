using JusMonitor.Domain.Entities;

namespace JusMonitor.Application.Common.Interfaces;

public interface IMovimentacaoRepository
{
    Task<bool> ExisteAsync(string descricao, DateTime data, Guid processoId, CancellationToken ct = default);
    Task AdicionarAsync(Movimentacao movimentacao, CancellationToken ct = default);
}