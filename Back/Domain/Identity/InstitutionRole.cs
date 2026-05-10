using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Domain.Identity;

/// <summary>
/// Define quais as roles da instituição.
/// </summary>
public class InstitutionRole
{
    public Guid InstitutionId { get; set; }
    public Guid RoleId { get; set; }

    public SykiRole Role { get; set; }

    public InstitutionRole() {}

    public InstitutionRole(Guid institutionId, SykiRole role)
    {
        InstitutionId = institutionId;
        Role = role;
    }
}
