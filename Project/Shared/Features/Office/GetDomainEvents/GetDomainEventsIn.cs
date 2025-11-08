namespace Exato.Shared.Features.Office.GetDomainEvents;

public class GetDomainEventsIn
{
    public int Page { get; set; }
    public int? OrganizationId { get; set; }
    public string? Type { get; set; }
    public DomainEventStatus? Status { get; set; }
    public CommandStatus? CommandStatus { get; set; }
}
