using Microsoft.AspNetCore.Identity;

namespace Syki.Back.Domain;

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
