namespace Estud.Back.Features.Identity.TwoFactorSetupLogin;

public class TwoFactorSetupLoginOut : IApiDto<TwoFactorSetupLoginOut>
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }

    public static IEnumerable<(string, TwoFactorSetupLoginOut)> GetExamples() =>
    [
        ("Exemplo",
        new TwoFactorSetupLoginOut
        {
            Id = 1,
            InstitutionId = 2,
        }),
    ];
}
