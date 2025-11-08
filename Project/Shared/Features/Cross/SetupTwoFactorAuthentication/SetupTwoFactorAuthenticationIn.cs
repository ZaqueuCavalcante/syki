namespace Exato.Shared.Features.Cross.SetupTwoFactorAuthentication;

public class SetupTwoFactorAuthenticationIn : IApiDto<SetupTwoFactorAuthenticationIn>
{
    public string Token { get; set; }

    public static IEnumerable<(string, SetupTwoFactorAuthenticationIn)> GetExamples() =>
    [
        ("Exemplo", new() { Token = "843972" }),
    ];
}
