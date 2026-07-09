namespace Estud.Back.Features.Identity.TwoFactorLogin;

public class TwoFactorLoginIn : IApiDto<TwoFactorLoginIn>
{
    /// <summary>
    /// Token gerado a partir da chave 2FA, utilizando o Authenticator App.
    /// </summary>
    public string? Token { get; set; }

    public static IEnumerable<(string, TwoFactorLoginIn)> GetExamples() =>
    [
        ("Exemplo", new() { Token = "853941" }),
    ];
}
