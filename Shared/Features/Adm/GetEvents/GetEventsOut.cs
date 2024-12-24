namespace Syki.Shared;

public class GetEventsOut
{
    public EventsSummaryOut Summary { get; set; } = new();

    public List<LastEventOut> LastEvents { get; set; } = [];

    public List<EventTypeCountOut> EventTypes { get; set; } = [];

    public List<TinyInstitutionOut> Institutions { get; set; } = [];
}
