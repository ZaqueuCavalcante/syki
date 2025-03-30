namespace Syki.Shared;

public class BatchOut
{
    public Guid Id { get; set; }
    public CommandBatchType Type { get; set; }
    public CommandBatchStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public Guid? EventId { get; set; }
    public Guid? SourceCommandId { get; set; }
    public Guid? NextCommandId { get; set; }
    public int Size { get; set; }

    public List<CommandOut> Commands { get; set; } = [];
}
