namespace Syki.Back.Auth.Users;

public class RequestUser
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public int InstitutionId { get; set; }
    public List<int> Permissions { get; set; }
}
