namespace Estud.Back.Shared;

public class CreatePendingUserRegisterIn : IApiDto<CreatePendingUserRegisterIn>
{
    public string Email { get; set; }

    public CreatePendingUserRegisterIn(string email)
    {
        Email = email;
    }

    public static IEnumerable<(string, CreatePendingUserRegisterIn)> GetExamples() =>
    [
        ("Acadêmico", new("academico@estud.com")),
        ("Professor", new("professor@estud.com")),
        ("Aluno", new("aluno@estud.com")),
    ];
}
