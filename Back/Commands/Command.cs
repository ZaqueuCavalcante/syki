namespace Syki.Back.Commands;

public class Command
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public CommandStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Duration { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public Guid? ProcessorId { get; set; }
    public string? Error { get; set; }
    public Guid? EventId { get; set; }
    public Guid? ParentId { get; set; }
    public Guid? BatchId { get; set; }

    public Command() { }

    public Command(Guid institutionId, object data, Guid? eventId, Guid? batchId)
    {
        Id = Guid.NewGuid();
        InstitutionId = institutionId;
        Type = data.GetType().ToString();
        Data = data.Serialize();
        CreatedAt = DateTime.Now;
        EventId = eventId;
        BatchId = batchId;
    }

    public void SetAwaiting(Guid batchId)
    {
        BatchId = batchId;
        Status = CommandStatus.Awaiting;
    }

    public void Processed(double duration)
    {
        ProcessedAt = DateTime.Now;
        Duration = Convert.ToInt32(duration);
        Status = Error.HasValue() ? CommandStatus.Error : CommandStatus.Success;
    }
}
