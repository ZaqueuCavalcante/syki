using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Commands;

public class CommandDbConfig : IEntityTypeConfiguration<Command>
{
    public void Configure(EntityTypeBuilder<Command> entity)
    {
        entity.ToTable("commands", "exato");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.Property(e => e.Status)
            .HasConversion<string>();

        entity.Property(e => e.CreatedAt)
            .HasColumnType("timestamp");

        entity.Property(e => e.ProcessedAt)
            .HasColumnType("timestamp");

        entity.Property(e => e.NotBefore)
            .HasColumnType("timestamp");
    }
}
