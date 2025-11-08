namespace Syki.Shared;

public class LoginIn : IApiDto<LoginIn>
{
    public string Email { get; set; }
    public string Password { get; set; }

    public LoginIn(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public static IEnumerable<(string, LoginIn)> GetExamples() =>
    [
        ("Acadêmico", new("academico@syki.com", "M1@Str0ngP4ssword#")),
        ("Professor", new("professor@syki.com", "M1@Str0ngP4ssword#")),
        ("Aluno", new("aluno@syki.com", "M1@Str0ngP4ssword#")),
    ];
}
