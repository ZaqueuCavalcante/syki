namespace Syki.Back.Domain.Identity;

public class SykiRole : IdentityRole<int>
{
    public int? OwnerId { get; set; }
    public string Description { get; set; }
    public List<int> Permissions { get; set; }

    private SykiRole() {}

    public SykiRole(
        int id,
        string name,
        string description,
        List<int> permissions)
    {
        Id = id;
        Name = name;
        NormalizedName = name.ToUpper();
        Description = description;
        Permissions = permissions;
        ConcurrencyStamp = id.ToString();
    }

    public bool IsSubsetOf(List<int> permissions)
    {
        return Permissions.IsSubsetOf(permissions);
    }
}
