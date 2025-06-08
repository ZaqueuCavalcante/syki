using StronglyTypedIds;

namespace Syki.Back.Events;

public class DomainEvent
{
    public DomainEventId Id { get; set; }
    public Guid InstitutionId { get; set; }
    public Guid EntityId { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public DomainEventStatus Status { get; set; }
    public DateTime OccurredAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public Guid? ProcessorId { get; set; }
    public string? Error { get; set; }
    public int Duration { get; set; }

    public DomainEvent() { }

    public DomainEvent(Guid institutionId, Guid entityId, object data)
    {
        Id = DomainEventId.CreateNew();
        EntityId = entityId;
        Type = data.GetType().ToString();
        Data = data.Serialize();
        OccurredAt = DateTime.UtcNow;
        InstitutionId = institutionId;
    }

    public void Processed(double duration)
    {
        ProcessedAt = DateTime.UtcNow;
        Duration = Convert.ToInt32(duration);
        Status = Error.HasValue() ? DomainEventStatus.Error : DomainEventStatus.Success;
    }
}

[StronglyTypedId]
public partial struct DomainEventId
{
    public static DomainEventId CreateNew()
    {
        return new DomainEventId(Guid.CreateVersion7());
    }

    public class DomainEventIdEfCoreValueConverter : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DomainEventId, Guid>
    {
        public DomainEventIdEfCoreValueConverter() : this(null) { }

        public DomainEventIdEfCoreValueConverter(Microsoft.EntityFrameworkCore.Storage.ValueConversion.ConverterMappingHints? mappingHints = null)
            : base(
                id => id.Value,
                value => new DomainEventId(value),
                mappingHints
            )
        { }
    }
}
