using JusMonitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JusMonitor.Infrastructure.Persistence.Configurations;

public class AdvogadoConfiguration : IEntityTypeConfiguration<Advogado>
{
    public void Configure(EntityTypeBuilder<Advogado> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Nome).IsRequired().HasMaxLength(200);
        builder.Property(a => a.Email).IsRequired().HasMaxLength(200);
        builder.Property(a => a.NumeroOAB).IsRequired().HasMaxLength(20);
        builder.Property(a => a.UfOAB).IsRequired().HasMaxLength(2);
        builder.Property(a => a.SenhaHash).IsRequired();

        builder.HasIndex(a => a.Email).IsUnique();
        builder.HasIndex(a => new { a.NumeroOAB, a.UfOAB }).IsUnique();

        builder.HasMany(a => a.Processos)
               .WithOne(p => p.Advogado)
               .HasForeignKey(p => p.AdvogadoId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}