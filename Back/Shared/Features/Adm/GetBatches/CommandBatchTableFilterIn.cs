namespace Syki.Back.Shared;

public class CommandBatchTableFilterIn
{
    public int? InstitutionId { get; set; }
    public CommandBatchType? Type { get; set; }
    public CommandBatchStatus? Status { get; set; }
}
