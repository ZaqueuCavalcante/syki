namespace Syki.Shared;

public class CreatePendingUserRegisterIn
{
    public string Email { get; set; }

    public CreatePendingUserRegisterIn(string email)
    {
        Email = email;
    }

    public static IEnumerable<(string, CreatePendingUserRegisterIn)> GetExamples() =>
    [
        ("Acadêmico", new("academico@syki.com")),
        ("Professor", new("professor@syki.com")),
        ("Aluno", new("aluno@syki.com")),
    ];
}
