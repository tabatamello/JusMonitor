using JusMonitor.Domain.Entities;

namespace JusMonitor.Application.Common.Interfaces;

public interface IProcessoRepository
{
    Task<Processo?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
    Task<Processo?> ObterPorNumeroAsync(string numeroProcesso, CancellationToken ct = default);
    Task<IEnumerable<Processo>> ListarPorAdvogadoAsync(Guid advogadoId, CancellationToken ct = default);
    Task<IEnumerable<Processo>> ListarParaMonitorarAsync(CancellationToken ct = default);
    Task AdicionarAsync(Processo processo, CancellationToken ct = default);
}