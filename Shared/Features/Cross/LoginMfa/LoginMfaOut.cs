namespace Syki.Shared;

public class LoginMfaOut
{
    public string AccessToken { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
}
