using JusMonitor.Application.Common.Interfaces;

namespace JusMonitor.Infrastructure.Persistence;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public Task<int> CommitAsync(CancellationToken ct = default)
        => context.SaveChangesAsync(ct);
}