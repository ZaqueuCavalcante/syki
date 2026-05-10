namespace Syki.Back.Auth.Users;

public class RequestUser
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public Guid InstitutionId { get; set; }
    public List<int> Permissions { get; set; }
}
