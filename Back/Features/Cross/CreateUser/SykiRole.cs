namespace Syki.Back.Features.Cross.CreateUser;

public class SykiRole : IdentityRole<Guid>
{
    private SykiRole() {}

    public SykiRole(UserRole role)
    {
        Id = Guid.NewGuid();
        Name = role.ToString();
        NormalizedName = role.ToString().ToUpper();
        ConcurrencyStamp = Guid.NewGuid().ToString();
    }
}
