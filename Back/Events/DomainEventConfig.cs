namespace Syki.Back.Events;

public class DomainEventConfig : IEntityTypeConfiguration<DomainEvent>
{
    public void Configure(EntityTypeBuilder<DomainEvent> domainEvent)
    {
        domainEvent.ToTable("domain_events");

        domainEvent.HasKey(e => e.Id);
        domainEvent.Property(e => e.Id).ValueGeneratedNever();
    }
}
