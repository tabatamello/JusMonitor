using JusMonitor.Application.Common.Interfaces;
using JusMonitor.Domain.Entities;
using JusMonitor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JusMonitor.Infrastructure.Repositories;

public class ProcessoRepository(AppDbContext context) : IProcessoRepository
{
    public Task<Processo?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
        => context.Processos.FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<Processo?> ObterPorNumeroAsync(string numeroProcesso, CancellationToken ct = default)
        => context.Processos.FirstOrDefaultAsync(p => p.NumeroProcesso == numeroProcesso, ct);

    public Task<IEnumerable<Processo>> ListarPorAdvogadoAsync(Guid advogadoId, CancellationToken ct = default)
        => Task.FromResult<IEnumerable<Processo>>(
            context.Processos.Where(p => p.AdvogadoId == advogadoId));

    public Task<IEnumerable<Processo>> ListarParaMonitorarAsync(CancellationToken ct = default)
        => Task.FromResult<IEnumerable<Processo>>(
            context.Processos.Where(p => p.Status == Domain.Enums.StatusProcesso.Ativo));

    public async Task AdicionarAsync(Processo processo, CancellationToken ct = default)
        => await context.Processos.AddAsync(processo, ct);
}