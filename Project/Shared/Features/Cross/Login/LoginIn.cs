namespace Exato.Shared.Features.Cross.Login;

public class LoginIn : IApiDto<LoginIn>
{
    public string Email { get; set; }
    public string Password { get; set; }

    public static IEnumerable<(string, LoginIn)> GetExamples() =>
    [
        ("Exemplo", new LoginIn { Email = "user@exato.com", Password = "M1@Str0ngP4ssword#" }),
    ];
}
