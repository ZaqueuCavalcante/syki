namespace Syki.Shared;

public class LoginMfaIn : IApiDto<LoginMfaIn>
{
    /// <summary>
    /// Token gerado a partir da chave MFA (utilizando o Google Authenticator, por exemplo).
    /// </summary>
    public string Token { get; set; }

    public static IEnumerable<(string, LoginMfaIn)> GetExamples() =>
    [
        ("Exemplo", new() { Token = "853941" }),
    ];
}
