namespace Exato.Shared.Features.Cross.TwoFactorAuthenticationLogin;

public class TwoFactorAuthenticationLoginIn : IApiDto<TwoFactorAuthenticationLoginIn>
{
    /// <summary>
    /// Token gerado a partir da chave 2FA, utilizando o Authenticator App.
    /// </summary>
    public string Token { get; set; }

    public static IEnumerable<(string, TwoFactorAuthenticationLoginIn)> GetExamples() =>
    [
        ("Exemplo", new() { Token = "853941" }),
    ];
}
