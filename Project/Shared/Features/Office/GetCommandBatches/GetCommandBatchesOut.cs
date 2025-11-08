namespace Exato.Shared.Features.Office.GetCommandBatches;

public class GetCommandBatchesOut
{
    public int Total { get; set; }
    public List<GetCommandBatchesItemOut> Items { get; set; } = [];
}

public class GetCommandBatchesItemOut
{
    public Guid Id { get; set; }
    public CommandBatchType Type { get; set; }
    public CommandBatchStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public int Size { get; set; }
}
