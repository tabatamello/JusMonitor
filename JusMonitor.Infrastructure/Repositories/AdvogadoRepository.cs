using JusMonitor.Application.Common.Interfaces;
using JusMonitor.Domain.Entities;
using JusMonitor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JusMonitor.Infrastructure.Repositories;

public class AdvogadoRepository(AppDbContext context) : IAdvogadoRepository
{
    public Task<Advogado?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
        => context.Advogados.FirstOrDefaultAsync(a => a.Id == id, ct);

    public Task<Advogado?> ObterPorEmailAsync(string email, CancellationToken ct = default)
        => context.Advogados.FirstOrDefaultAsync(a => a.Email == email, ct);

    public Task<Advogado?> ObterPorOABAsync(string numeroOAB, string uf, CancellationToken ct = default)
        => context.Advogados.FirstOrDefaultAsync(a => a.NumeroOAB == numeroOAB && a.UfOAB == uf, ct);

    public Task<bool> ExisteEmailAsync(string email, CancellationToken ct = default)
        => context.Advogados.AnyAsync(a => a.Email == email, ct);

    public async Task AdicionarAsync(Advogado advogado, CancellationToken ct = default)
        => await context.Advogados.AddAsync(advogado, ct);
}
