using Syki.Back.Domain.Enums;

namespace Syki.Back.Shared;

public class CommandTableFilterIn
{
    public Guid? InstitutionId { get; set; }
    public string? Type { get; set; }
    public CommandStatus? Status { get; set; }
}
