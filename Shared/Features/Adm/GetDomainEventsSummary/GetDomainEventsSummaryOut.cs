namespace Syki.Shared;

public class GetDomainEventsSummaryOut
{
    public DomainEventsSummaryOut Summary { get; set; } = new();

    public List<LastDomainEventOut> LastEvents { get; set; } = [];

    public List<DomainEventTypeCountOut> EventTypes { get; set; } = [];

    public List<TinyInstitutionOut> Institutions { get; set; } = [];
}
