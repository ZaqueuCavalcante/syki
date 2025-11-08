namespace Exato.Back.Features.Cross.CreateExatoUser;

public class ExatoUserRole : IdentityUserRole<Guid>
{
    private ExatoUserRole() {}

    public ExatoUserRole(Guid userId, Guid roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }
}
