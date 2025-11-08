namespace Exato.Back.Features.Cross.CreateExatoUser;

public class ExatoRole : IdentityRole<Guid>
{
    public string Description { get; set; }
    public int OrganizationId { get; set; }
    public List<int> Features { get; set; }

    private ExatoRole() { }

    public ExatoRole(
        string name,
        string description,
        int organizationId,
        List<int> features)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Features = features;
        OrganizationId = organizationId;
        NormalizedName = name.ToUpper();
        ConcurrencyStamp = Id.ToString();
    }

    public void Update(
        string name,
        string description,
        List<int> features)
    {
        Name = name;
        Description = description;
        Features = features;
        NormalizedName = name.ToUpper();
    }
}
