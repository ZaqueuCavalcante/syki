namespace Syki.Back.Events;

public class DomainEvent
{
    public Guid Id { get; set; }
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
        Id = Guid.CreateVersion7();
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
