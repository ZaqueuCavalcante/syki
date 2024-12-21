namespace Syki.Back.Tasks;

public class SykiTask
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string? Error { get; set; }
    public Guid? ProcessorId { get; set; }
    public Guid? EventId { get; set; }

    public SykiTask() { }

    public SykiTask(object data)
    {
        Id = Guid.NewGuid();
        Type = data.GetType().ToString();
        Data = data.Serialize();
        CreatedAt = DateTime.Now;
    }

    public TaskOut ToOut()
    {
        return new()
        {
            Id = Id,
            Type = Type,
            Data = Data,
            CreatedAt = CreatedAt,
            ProcessedAt = ProcessedAt,
            Error = Error,
        };
    }
}
