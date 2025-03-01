namespace Syki.Shared;

public class DomainEventCommandOut
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public CommandStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public int Duration { get; set; }
}
