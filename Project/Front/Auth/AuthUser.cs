namespace Exato.Front.Auth;

public class AuthUser
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public bool IsAuthenticated { get; set; }
    public List<int> Features { get; set; } = [];
}
