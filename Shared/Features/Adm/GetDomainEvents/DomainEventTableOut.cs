namespace Syki.Shared;

public class DomainEventTableOut
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public DomainEventStatus Status { get; set; }
    public DateTime OccurredAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public CommandStatus[] Commands { get; set; } = [];
}
