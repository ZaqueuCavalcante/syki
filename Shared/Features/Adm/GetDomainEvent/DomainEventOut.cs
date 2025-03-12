namespace Syki.Shared;

public class DomainEventOut
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public DomainEventStatus Status { get; set; }
    public DateTime OccurredAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string? Error { get; set; }
    public int Duration { get; set; }

    public List<DomainEventCommandOut> Commands { get; set; } = [];
}
