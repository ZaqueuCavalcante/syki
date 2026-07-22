using Estud.Back.Domain.Institutions;

namespace Estud.Back.Domain.Identity;

public class EstudUserRole : IdentityUserRole<int>
{
    public int InstitutionId { get; set; }

    public Institution? Institution { get; set; }
    public EstudUser? User { get; set; }
    public EstudRole? Role { get; set; }

    public EstudUserRole() {}

    public EstudUserRole(Institution institution, EstudUser user, EstudRole role)
    {
        Institution = institution;
        User = user;
        Role = role;
    }

    public EstudUserRole(Institution institution, EstudUser user, int roleId)
    {
        Institution = institution;
        User = user;
        RoleId = roleId;
    }

    public EstudUserRole(int institutionId, EstudUser user, int roleId)
    {
        InstitutionId = institutionId;
        User = user;
        RoleId = roleId;
    }

    public EstudUserRole(int institutionId, int userId, int roleId)
    {
        InstitutionId = institutionId;
        UserId = userId;
        RoleId = roleId;
    }
}
