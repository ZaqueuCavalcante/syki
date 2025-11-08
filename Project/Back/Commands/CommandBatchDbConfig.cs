using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Commands;

public class CommandBatchDbConfig : IEntityTypeConfiguration<CommandBatch>
{
    public void Configure(EntityTypeBuilder<CommandBatch> entity)
    {
        entity.ToTable("command_batches", "exato");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.Property(e => e.Type)
            .HasConversion<string>();

        entity.Property(e => e.Status)
            .HasConversion<string>();

        entity.Property(e => e.CreatedAt)
            .HasColumnType("timestamp");

        entity.Property(e => e.ProcessedAt)
            .HasColumnType("timestamp");
    }
}
