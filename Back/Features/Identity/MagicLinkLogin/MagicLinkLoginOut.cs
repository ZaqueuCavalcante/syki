namespace Syki.Back.Features.Identity.MagicLinkLogin;

public class MagicLinkLoginOut : IApiDto<MagicLinkLoginOut>
{
    public Guid UserId { get; set; }
    public Guid InstitutionId { get; set; }
    public List<int> Permissions { get; set; } = [];

    public static IEnumerable<(string, MagicLinkLoginOut)> GetExamples() =>
    [
        ("Exemplo",
        new MagicLinkLoginOut
        {
            UserId = Guid.NewGuid(),
            Permissions = [1, 2, 3, 4],
            InstitutionId = Guid.NewGuid(),
        }),
    ];
}
