using JusMonitor.Domain.Entities;

namespace JusMonitor.Application.Common.Interfaces;

public interface IAdvogadoRepository
{
    Task<Advogado?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
    Task<Advogado?> ObterPorEmailAsync(string email, CancellationToken ct = default);
    Task<Advogado?> ObterPorOABAsync(string numeroOAB, string uf, CancellationToken ct = default);
    Task<bool> ExisteEmailAsync(string email, CancellationToken ct = default);
    Task AdicionarAsync(Advogado advogado, CancellationToken ct = default);
}