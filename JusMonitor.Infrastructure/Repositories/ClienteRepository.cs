using JusMonitor.Application.Common.Interfaces;
using JusMonitor.Domain.Entities;
using JusMonitor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JusMonitor.Infrastructure.Repositories;

public class ClienteRepository(AppDbContext context) : IClienteRepository
{
    public Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
        => context.Clientes.FirstOrDefaultAsync(c => c.Id == id, ct);

    public Task<Cliente?> ObterPorEmailAsync(string email, CancellationToken ct = default)
        => context.Clientes.FirstOrDefaultAsync(c => c.Email == email, ct);

    public Task<bool> ExisteEmailAsync(string email, CancellationToken ct = default)
        => context.Clientes.AnyAsync(c => c.Email == email, ct);

    public async Task AdicionarAsync(Cliente cliente, CancellationToken ct = default)
        => await context.Clientes.AddAsync(cliente, ct);
}