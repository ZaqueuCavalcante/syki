namespace Syki.Shared;

public class DomainEventTableFilterIn
{
    public Guid? InstitutionId { get; set; }
    public string? Type { get; set; }
    public DomainEventStatus? Status { get; set; }
    public SykiTaskStatus? Tasks { get; set; }
}
