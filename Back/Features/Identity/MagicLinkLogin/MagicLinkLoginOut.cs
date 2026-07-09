namespace Estud.Back.Features.Identity.MagicLinkLogin;

public class MagicLinkLoginOut : IApiDto<MagicLinkLoginOut>
{
    public int UserId { get; set; }
    public int InstitutionId { get; set; }

    public static IEnumerable<(string, MagicLinkLoginOut)> GetExamples() =>
    [
        ("Exemplo",
        new MagicLinkLoginOut
        {
            UserId = 1,
            InstitutionId = 1,
        }),
    ];
}
