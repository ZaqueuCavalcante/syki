namespace Syki.Back.Features.Cross.CreateUser;

public class SykiRole : IdentityRole<Guid>
{
    public SykiRole(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        NormalizedName = name.ToUpper();
        ConcurrencyStamp = Guid.NewGuid().ToString();
    }
}
