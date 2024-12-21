namespace Syki.Shared;

public class DomainEventTaskOut
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public SykiTaskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public int Duration { get; set; }
}
