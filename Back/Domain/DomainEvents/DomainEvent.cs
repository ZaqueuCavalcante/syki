using System.Diagnostics;

namespace Estud.Back.Domain.DomainEvents;

public class DomainEvent
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public string EntityUid { get; set; }
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

    public DomainEvent(int institutionId, string entityUid, object data, string? activityId)
    {
        EntityUid = entityUid;
        Type = data.GetType().ToString();
        Data = data.Serialize();
        OccurredAt = DateTime.UtcNow;
        InstitutionId = institutionId;
        ActivityId = activityId;
    }

    public void Processed(double duration)
    {
        ProcessedAt = DateTime.UtcNow;
        Duration = Convert.ToInt32(duration);
        Status = Error.HasValue() ? DomainEventStatus.Error : DomainEventStatus.Success;
    }

    public ActivityContext GetParentContext()
    {
        ActivityContext.TryParse(ActivityId, null, out var parsedContext);
        return parsedContext;
    }
}
