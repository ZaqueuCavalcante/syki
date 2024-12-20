namespace Syki.Back.Events;

public class DomainEvent
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string? Error { get; set; }
    public Guid? ProcessorId { get; set; }

    public DomainEvent() { }

    public DomainEvent(object data)
    {
        Id = Guid.NewGuid();
        Type = data.GetType().ToString();
        Data = data.Serialize();
        CreatedAt = DateTime.Now;
    }
}
