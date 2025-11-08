using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Events;

public class DomainEventDbConfig : IEntityTypeConfiguration<DomainEvent>
{
    public void Configure(EntityTypeBuilder<DomainEvent> entity)
    {
        entity.ToTable("domain_events", "exato");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.Property(e => e.Status)
            .HasConversion<string>();

        entity.Property(e => e.OccurredAt)
            .HasColumnType("timestamp");

        entity.Property(e => e.ProcessedAt)
            .HasColumnType("timestamp");
    }
}
