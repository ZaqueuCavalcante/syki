namespace Syki.Shared;

public class LoginOut
{
    public string AccessToken { get; set; }
    public string TwoFactorUserId { get; set; }
    public bool RequiresTwoFactor { get; set; }
    public bool WrongEmailOrPassword { get; set; }
    public bool Wrong2FactorCode { get; set; }
}
