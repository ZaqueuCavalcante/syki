namespace Estud.Back.Domain.Identity;

public class EstudRole : IdentityRole<int>
{
    public int InstitutionId { get; set; }
    public string Description { get; set; }
    public UserType BaseType { get; set; }
    public List<int> Permissions { get; set; }
    public bool TwoFactorRequired { get; set; }

    public EstudRole() { }

    public EstudRole(
        int institutionId,
        string name,
        string description,
        UserType baseType,
        List<int> permissions)
    {
        InstitutionId = institutionId;
        Name = name;
        NormalizedName = name.ToUpper();
        Description = description;
        BaseType = baseType;
        Permissions = permissions;
        ConcurrencyStamp = Guid.NewGuid().ToString();
    }

    public void Update(
        string name,
        string normalizedName,
        string description,
        List<int> permissions)
    {
        Name = name;
        NormalizedName = normalizedName;
        Description = description;
        Permissions = permissions;
    }

    public void SetTwoFactorRequired(bool required)
    {
        TwoFactorRequired = required;
    }

    public bool IsSubsetOf(List<int> permissions)
    {
        return Permissions.IsSubsetOf(permissions);
    }
}
