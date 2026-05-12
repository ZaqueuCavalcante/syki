namespace Syki.Back.Features.Identity.MagicLinkLogin;

public class MagicLinkLoginOut : IApiDto<MagicLinkLoginOut>
{
    public int UserId { get; set; }
    public int InstitutionId { get; set; }
    public List<int> Permissions { get; set; } = [];

    public static IEnumerable<(string, MagicLinkLoginOut)> GetExamples() =>
    [
        ("Exemplo",
        new MagicLinkLoginOut
        {
            UserId = 1,
            InstitutionId = 1,
            Permissions = [1, 2, 3, 4],
        }),
    ];
}
