namespace Syki.Shared;

public class CommandBatchTableFilterIn
{
    public Guid? InstitutionId { get; set; }
    public CommandBatchType? Type { get; set; }
    public CommandBatchStatus? Status { get; set; }
}
