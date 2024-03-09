namespace Syki.Shared;

public class LoginOut
{
    public string AccessToken { get; set; }
    public bool RequiresTwoFactor { get; set; }
    public bool WrongEmailOrPassword { get; set; }
}
