namespace Syki.Back.Features.Identity.GoogleOneTapLogin;

public class GoogleOneTapLoginOut : IApiDto<GoogleOneTapLoginOut>
{
    public int UserId { get; set; }
    public int InstitutionId { get; set; }
    public List<int> Permissions { get; set; } = [];

    public static IEnumerable<(string Name, GoogleOneTapLoginOut Value)> GetExamples() =>
    [
        ("Exemplo",
        new GoogleOneTapLoginOut
        {
            UserId = 1,
            InstitutionId = 1,
            Permissions = [1, 2, 3],
        }),
    ];
}
