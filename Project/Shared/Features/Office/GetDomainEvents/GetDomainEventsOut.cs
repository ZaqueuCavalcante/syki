namespace Exato.Shared.Features.Office.GetDomainEvents;

public class GetDomainEventsOut
{
    public int Total { get; set; }
    public List<string> Types { get; set; } = [];
    public List<GetDomainEventsItemOut> Items { get; set; } = [];
}

public class GetDomainEventsItemOut
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public DomainEventStatus Status { get; set; }
    public DateTime OccurredAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public CommandStatus[] Commands { get; set; } = [];
}
