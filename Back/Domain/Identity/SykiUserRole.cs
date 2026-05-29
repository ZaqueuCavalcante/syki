using Syki.Back.Domain.Institutions;

namespace Syki.Back.Domain.Identity;

public class SykiUserRole : IdentityUserRole<int>
{
    public int InstitutionId { get; set; }

    public Institution? Institution { get; set; }
    public SykiUser? User { get; set; }
    public SykiRole? Role { get; set; }

    public SykiUserRole() {}

    public SykiUserRole(Institution institution, SykiUser user, int roleId)
    {
        Institution = institution;
        User = user;
        RoleId = roleId;
    }

    public SykiUserRole(int institutionId, SykiUser user, int roleId)
    {
        InstitutionId = institutionId;
        User = user;
        RoleId = roleId;
    }

    public SykiUserRole(int institutionId, int userId, int roleId)
    {
        InstitutionId = institutionId;
        UserId = userId;
        RoleId = roleId;
    }
}
