using JusMonitor.Application.Common.Interfaces;
using JusMonitor.Domain.Entities;
using JusMonitor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JusMonitor.Infrastructure.Repositories;

public class MovimentacaoRepository(AppDbContext context) : IMovimentacaoRepository
{
    public Task<bool> ExisteAsync(string descricao, DateTime data, Guid processoId, CancellationToken ct = default)
        => context.Movimentacoes.AnyAsync(
            m => m.ProcessoId == processoId &&
                 m.DataMovimentacao == data &&
                 m.Descricao == descricao, ct);

    public async Task AdicionarAsync(Movimentacao movimentacao, CancellationToken ct = default)
        => await context.Movimentacoes.AddAsync(movimentacao, ct);
}