namespace Exato.Back.Features.Cross.CreateExatoUser;

public class ExatoUser : IdentityUser<Guid>
{
    public int OrganizationId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Apenas para facilitar nas queries. <br/>
    /// Propriedade não mapeada pelo EF.
    /// </summary>
    public string Role { get; set; }

    public ExatoUser() { }

    public ExatoUser(
        int organizationId,
        string name,
        string email)
    {
        OrganizationId = organizationId;
        Name = name;
        UserName = email;
        Email = email;
        CreatedAt = DateTime.Now;
    }
}
