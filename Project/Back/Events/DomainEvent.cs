using System.Diagnostics;

namespace Exato.Back.Events;

public class DomainEvent
{
    public Guid Id { get; set; }
    public int OrganizationId { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public DomainEventStatus Status { get; set; }
    public DateTime OccurredAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public Guid? ProcessorId { get; set; }
    public string? Error { get; set; }
    public int Duration { get; set; }
    public string? ActivityId { get; set; }

    public DomainEvent() { }

    public DomainEvent(int organizationId, object data, string? activityId)
    {
        Id = Guid.CreateVersion7();
        Type = data.GetType().ToString();
        Data = data.Serialize();
        OccurredAt = DateTime.Now;
        OrganizationId = organizationId;
        ActivityId = activityId;
    }

    public void Processed(double duration)
    {
        ProcessedAt = DateTime.Now;
        Duration = Convert.ToInt32(duration);
        Status = Error.HasValue() ? DomainEventStatus.Error : DomainEventStatus.Success;
    }

    public ActivityContext GetParentContext()
    {
        ActivityContext.TryParse(ActivityId, null, out var parsedContext);
        return parsedContext;
    }
}
