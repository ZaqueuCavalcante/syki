namespace Syki.Back.Events;

public class DomainEvent
{
    public Guid Id { get; set; }
    public Guid EntityId { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string? Error { get; set; }

    public DomainEvent() { }

    public DomainEvent(
        Guid entityId,
        object data)
    {
        Id = Guid.NewGuid();
        EntityId = entityId;
        Type = data.GetType().ToString();
        Data = data.Serialize();
        CreatedAt = DateTime.Now;
    }
}
