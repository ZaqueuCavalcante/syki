namespace Estud.Back.Features.Identity.EmailPasswordLogin;

public class EmailPasswordLoginIn : IApiDto<EmailPasswordLoginIn>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string? TurnstileToken { get; set; }

    public static IEnumerable<(string, EmailPasswordLoginIn)> GetExamples() =>
    [
        ("Exemplo", new EmailPasswordLoginIn { Email = "user@estud.com", Password = "M1@Str0ngP4ssword#" }),
    ];
}
