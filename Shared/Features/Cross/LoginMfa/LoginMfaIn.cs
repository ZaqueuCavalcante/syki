namespace Syki.Shared;

public class LoginMfaIn
{
    /// <summary>
    /// Token gerado a partir da chave MFA (utilizando o Google Authenticator, por exemplo).
    /// </summary>
    public string Token { get; set; }
}
