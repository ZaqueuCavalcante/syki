namespace Syki.Back.Domain.Identity;

public class SykiRole : IdentityRole<int>
{
    public int? OwnerId { get; set; }
    public string Description { get; set; }
    public List<int> Permissions { get; set; }

    public SykiRole() {}

    public SykiRole(
        int ownerId,
        string name,
        string description,
        List<int> permissions)
    {
        OwnerId = ownerId;
        Name = name;
        NormalizedName = name.ToUpper();
        Description = description;
        Permissions = permissions;
        ConcurrencyStamp = Guid.NewGuid().ToString();
    }

    public bool IsSubsetOf(List<int> permissions)
    {
        return Permissions.IsSubsetOf(permissions);
    }
}
