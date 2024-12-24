namespace Syki.Shared;

public class SykiTaskTableFilterIn
{
    public Guid? InstitutionId { get; set; }
    public string? Type { get; set; }
    public SykiTaskStatus? Status { get; set; }
}
