namespace Syki.Back.Features.Cross.CreateUser;

public class SykiRole : IdentityRole<Guid>
{
    public Guid? OwnerId { get; set; }
    public string Description { get; set; }
    public List<int> Permissions { get; set; }

    private SykiRole() {}

    public SykiRole(
        Guid id,
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
