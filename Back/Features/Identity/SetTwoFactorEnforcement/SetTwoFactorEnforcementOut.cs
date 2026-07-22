namespace Estud.Back.Features.Identity.SetTwoFactorEnforcement;

public class SetTwoFactorEnforcementOut : IApiDto<SetTwoFactorEnforcementOut>
{
    public int RoleId { get; set; }
    public bool TwoFactorRequired { get; set; }

    public static IEnumerable<(string, SetTwoFactorEnforcementOut)> GetExamples() =>
    [
        ("Exemplo", new SetTwoFactorEnforcementOut { RoleId = 1, TwoFactorRequired = true }),
    ];
}
