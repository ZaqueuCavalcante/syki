namespace Syki.Back.Domain.Identity;

public class SykiRole : IdentityRole<int>
{
    public int? OwnerId { get; set; }
    public string Description { get; set; }
    public UserType BaseType { get; set; }
    public List<int> Permissions { get; set; }

    public SykiRole() { }

    public SykiRole(
        int ownerId,
        string name,
        string description,
        UserType baseType,
        List<int> permissions)
    {
        OwnerId = ownerId;
        Name = name;
        NormalizedName = name.ToUpper();
        Description = description;
        BaseType = baseType;
        Permissions = permissions;
        ConcurrencyStamp = Guid.NewGuid().ToString();
    }

    public bool IsSubsetOf(List<int> permissions)
    {
        return Permissions.IsSubsetOf(permissions);
    }
}
