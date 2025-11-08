namespace Exato.Shared.Features.Office.GetCommandBatches;

public class GetCommandBatchesIn
{
    public int Page { get; set; }
    public CommandBatchType? Type { get; set; }
    public CommandBatchStatus? Status { get; set; }
}
