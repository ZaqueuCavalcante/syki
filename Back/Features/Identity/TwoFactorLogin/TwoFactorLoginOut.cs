namespace Syki.Back.Features.Identity.TwoFactorLogin;

public class TwoFactorLoginOut : IApiDto<TwoFactorLoginOut>
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }

    public static IEnumerable<(string, TwoFactorLoginOut)> GetExamples() =>
    [
        ("Exemplo",
        new TwoFactorLoginOut
        {
            Id = 1,
            InstitutionId = 2,
        }),
    ];
}
