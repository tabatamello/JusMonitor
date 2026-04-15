using JusMonitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JusMonitor.Infrastructure.Persistence.Configurations;

public class ProcessoConfiguration : IEntityTypeConfiguration<Processo>
{
    public void Configure(EntityTypeBuilder<Processo> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.NumeroProcesso).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Tribunal).IsRequired().HasMaxLength(20);
        builder.Property(p => p.Vara).HasMaxLength(200);
        builder.Property(p => p.Assunto).HasMaxLength(500);
        builder.Property(p => p.Status).HasConversion<string>();

        builder.HasIndex(p => p.NumeroProcesso).IsUnique();

        builder.HasMany(p => p.Movimentacoes)
               .WithOne(m => m.Processo)
               .HasForeignKey(m => m.ProcessoId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}