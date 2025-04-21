namespace Syki.Back.Features.Cross.CreateUser;

public class SykiRole : IdentityRole<Guid>
{
    private SykiRole() {}

    public SykiRole(Guid id, UserRole role)
    {
        Id = id;
        Name = role.ToString();
        NormalizedName = role.ToString().ToUpper();
        ConcurrencyStamp = id.ToString();
    }
}
