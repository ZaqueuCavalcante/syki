namespace Syki.Back.Features.Identity.EmailPasswordLogin;

public class EmailPasswordLoginOut : IApiDto<EmailPasswordLoginOut>
{
    public Guid UserId { get; set; }
    public Guid InstitutionId { get; set; }
    public List<int> Permissions { get; set; } = [];

    public static IEnumerable<(string, EmailPasswordLoginOut)> GetExamples() =>
    [
        ("Exemplo",
        new EmailPasswordLoginOut
        {
            UserId = Guid.NewGuid(),
            InstitutionId = Guid.NewGuid(),
            Permissions = [1, 2, 3],
        }),
    ];
}
