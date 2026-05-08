namespace Syki.Back.Features.Identity.MagicLinkLogin;

public class MagicLinkLoginIn : IApiDto<MagicLinkLoginIn>
{
    public string? Token { get; set; }

    public static IEnumerable<(string, MagicLinkLoginIn)> GetExamples() =>
    [
        ("Exemplo", new MagicLinkLoginIn { Token = Guid.NewGuid().ToString() }),
    ];
}
