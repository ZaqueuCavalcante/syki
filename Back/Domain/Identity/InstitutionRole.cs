namespace Syki.Back.Domain.Identity;

/// <summary>
/// Define quais as roles da instituição.
/// </summary>
public class InstitutionRole
{
    public int InstitutionId { get; set; }
    public int RoleId { get; set; }

    public SykiRole Role { get; set; }

    public InstitutionRole() {}

    public InstitutionRole(int institutionId, SykiRole role)
    {
        InstitutionId = institutionId;
        Role = role;
    }
}
