namespace Syki.Back.CreateUser;

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
