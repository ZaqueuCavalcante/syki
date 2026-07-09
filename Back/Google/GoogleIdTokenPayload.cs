namespace Estud.Back.Google;

public class GoogleIdTokenPayload
{
    public string Email { get; set; }
    public bool EmailVerified { get; set; }
    public string Subject { get; set; }
    public string? Name { get; set; }
}
