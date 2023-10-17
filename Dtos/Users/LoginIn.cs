namespace Syki.Dtos;

public class LoginIn
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string? TwoFactorToken { get; set; }
}
