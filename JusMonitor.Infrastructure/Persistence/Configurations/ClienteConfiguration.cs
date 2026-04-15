using JusMonitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JusMonitor.Infrastructure.Persistence.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Email).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Telefone).HasMaxLength(20);
        builder.Property(c => c.FcmToken).HasMaxLength(500);

        builder.HasIndex(c => c.Email).IsUnique();

        builder.HasMany(c => c.Processos)
               .WithOne(p => p.Cliente)
               .HasForeignKey(p => p.ClienteId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}