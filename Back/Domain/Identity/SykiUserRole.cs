using Syki.Back.Domain.Institutions;

namespace Syki.Back.Domain.Identity;

public class SykiUserRole : IdentityUserRole<int>
{
    /// <summary>
    /// Garantir que essa instituição sempre esteja na mesma sub-árvore da <see cref="SykiRole.OwnerId"/>. <br/>
    /// Assim as roles ficam scopadas e organizadas dentro da hierarquia de instituições.
    /// </summary>
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

    public SykiUserRole(int institutionId, int userId, int roleId)
    {
        InstitutionId = institutionId;
        UserId = userId;
        RoleId = roleId;
    }
}
