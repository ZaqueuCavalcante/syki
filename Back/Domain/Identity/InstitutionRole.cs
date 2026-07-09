namespace Estud.Back.Domain.Identity;

/// <summary>
/// Define quais as roles da instituição.
/// </summary>
public class InstitutionRole
{
    public int InstitutionId { get; set; }
    public int RoleId { get; set; }

    public EstudRole Role { get; set; }

    public InstitutionRole() {}

    public InstitutionRole(int institutionId, EstudRole role)
    {
        InstitutionId = institutionId;
        Role = role;
    }
}
