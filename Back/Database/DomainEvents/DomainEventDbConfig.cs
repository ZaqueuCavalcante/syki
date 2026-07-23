using Estud.Back.Domain.DomainEvents;

namespace Estud.Back.Database.DomainEvents;

public class DomainEventDbConfig : IEntityTypeConfiguration<DomainEvent>
{
    public void Configure(EntityTypeBuilder<DomainEvent> entity)
    {
        entity.ToTable("domain_events", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.Property(e => e.EntityUid).HasMaxLength(26);
    }
}
