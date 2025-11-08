namespace Exato.Shared.Features.Office.GetCommandBatch;

public class GetCommandBatchOut
{
    public Guid Id { get; set; }
    public CommandBatchType Type { get; set; }
    public CommandBatchStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public int Size { get; set; }
}
