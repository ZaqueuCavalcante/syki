namespace Estud.Back.Auth.Users;

public class RequestUser
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public List<int> Permissions { get; set; }
}
