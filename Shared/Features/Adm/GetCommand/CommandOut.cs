namespace Syki.Shared;

public class CommandOut
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public string Data { get; set; }
    public CommandStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string? Error { get; set; }
    public int Duration { get; set; }
    public Guid? EventId { get; set; }
    public Guid? ParentId { get; set; }
    public Guid? OriginalId { get; set; }
    public Guid? BatchId { get; set; }
    public Guid? SourceBatchId { get; set; }

    public List<CommandOut> Retries { get; set; } = [];
    public List<CommandOut> Subcommands { get; set; } = [];
    public List<BatchOut> Batches { get; set; } = [];
}
