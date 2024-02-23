namespace Syki.Shared.LoginMfa;

public class LoginMfaOut
{
    public string AccessToken { get; set; }
    public bool Wrong2FactorCode { get; set; }
}
