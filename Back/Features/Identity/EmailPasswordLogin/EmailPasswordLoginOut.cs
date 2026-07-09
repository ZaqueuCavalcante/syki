namespace Estud.Back.Features.Identity.EmailPasswordLogin;

public class EmailPasswordLoginOut : IApiDto<EmailPasswordLoginOut>
{
    public int UserId { get; set; }
    public int InstitutionId { get; set; }

    public static IEnumerable<(string, EmailPasswordLoginOut)> GetExamples() =>
    [
        ("Exemplo",
        new EmailPasswordLoginOut
        {
            UserId = 1,
            InstitutionId = 1,
        }),
    ];
}
