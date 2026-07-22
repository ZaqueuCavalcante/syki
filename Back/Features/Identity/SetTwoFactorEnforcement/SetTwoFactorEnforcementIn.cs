namespace Estud.Back.Features.Identity.SetTwoFactorEnforcement;

public class SetTwoFactorEnforcementIn : IApiDto<SetTwoFactorEnforcementIn>
{
    public int RoleId { get; set; }
    public bool Required { get; set; }

    public static IEnumerable<(string, SetTwoFactorEnforcementIn)> GetExamples() =>
    [
        ("Exemplo", new SetTwoFactorEnforcementIn { RoleId = 1, Required = true }),
    ];
}
