namespace Syki.Shared;

public class EventTableOut
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public DomainEventStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public int Duration { get; set; }
}
