using JusMonitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JusMonitor.Infrastructure.Persistence.Configurations;

public class MovimentacaoConfiguration : IEntityTypeConfiguration<Movimentacao>
{
    public void Configure(EntityTypeBuilder<Movimentacao> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Descricao).IsRequired().HasMaxLength(1000);
        builder.Property(m => m.CodigoMovimento).HasMaxLength(20);

        builder.HasIndex(m => new { m.ProcessoId, m.DataMovimentacao });
    }
}