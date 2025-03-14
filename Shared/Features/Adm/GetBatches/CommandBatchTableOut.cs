namespace Syki.Shared;

public class CommandBatchTableOut
{
    public Guid Id { get; set; }
    public int Size { get; set; }
    public CommandBatchType Type { get; set; }
    public CommandBatchStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
}
