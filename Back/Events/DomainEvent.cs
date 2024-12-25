namespace Syki.Back.Events;

public class DomainEvent
{
    public Guid Id { get; set; }
    public Guid EntityId { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public DomainEventStatus Status { get; set; }
    public DateTime OccurredAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public Guid? ProcessorId { get; set; }
    public string? Error { get; set; }
    public int Duration { get; set; }
    public Guid? ParentId { get; set; }
    public Guid InstitutionId { get; set; }

    public DomainEvent() { }

    public DomainEvent(Guid entityId, object data, Guid institutionId)
    {
        Id = Guid.NewGuid();
        EntityId = entityId;
        Type = data.GetType().ToString();
        Data = data.Serialize();
        OccurredAt = DateTime.Now;
        InstitutionId = institutionId;
    }
}
