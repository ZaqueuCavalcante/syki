namespace Syki.Shared;

public class SykiTaskTableFilterIn
{
    public Guid? InstitutionId { get; set; }
    public string? Type { get; set; }
    public CommandStatus? Status { get; set; }
}
