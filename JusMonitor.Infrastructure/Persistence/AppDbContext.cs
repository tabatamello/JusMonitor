using JusMonitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JusMonitor.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Advogado> Advogados => Set<Advogado>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Processo> Processos => Set<Processo>();
    public DbSet<Movimentacao> Movimentacoes => Set<Movimentacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}