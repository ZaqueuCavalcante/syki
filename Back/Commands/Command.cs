namespace Syki.Back.Commands;

public class Command
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public CommandStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public Guid? ProcessorId { get; set; }
    public string? Error { get; set; }
    public Guid? EventId { get; set; }
    public int Duration { get; set; }
    public Guid? ParentId { get; set; }

    public Command() { }

    public Command(Guid? eventId, Guid institutionId, object data)
    {
        Id = Guid.NewGuid();
        Type = data.GetType().ToString();
        Data = data.Serialize();
        CreatedAt = DateTime.Now;
        EventId = eventId;
        InstitutionId = institutionId;
    }
}
