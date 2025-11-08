namespace Syki.Shared;

public class SendResetPasswordTokenIn : IApiDto<SendResetPasswordTokenIn>
{
    public string Email { get; set; }

    public SendResetPasswordTokenIn() {}

    public SendResetPasswordTokenIn(string email)
    {
        Email = email;
    }

    public static IEnumerable<(string, SendResetPasswordTokenIn)> GetExamples() =>
    [
        ("Acadêmico", new() { Email = "academico@syki.com" }),
        ("Professor", new() { Email = "professor@syki.com" }),
        ("Aluno", new() { Email = "aluno@syki.com" }),
    ];
}
