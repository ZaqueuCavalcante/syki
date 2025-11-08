namespace Exato.Shared.Features.Cross.CreateExatoUser;

public class CreateExatoUserIn
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Guid RoleId { get; set; }
    public int OrganizationId { get; set; }
}
