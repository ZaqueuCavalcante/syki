namespace Syki.Back.Features.Identity.SetupTwoFactor;

public class SetupTwoFactorIn : IApiDto<SetupTwoFactorIn>
{
    public string Token { get; set; }

    public static IEnumerable<(string, SetupTwoFactorIn)> GetExamples() =>
    [
        ("Exemplo", new() { Token = "843972" }),
    ];
}
