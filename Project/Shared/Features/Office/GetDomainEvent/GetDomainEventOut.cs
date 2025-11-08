namespace Exato.Shared.Features.Office.GetDomainEvent;

public class GetDomainEventOut
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public string Description { get; set; }
    public DomainEventStatus Status { get; set; }
    public DateTime OccurredAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public int Duration { get; set; }
    public string? Error { get; set; }
    public List<GetDomainEventOutCommandOut> Commands { get; set; } = [];
}

public class GetDomainEventOutCommandOut
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public CommandStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public int Duration { get; set; }
}
