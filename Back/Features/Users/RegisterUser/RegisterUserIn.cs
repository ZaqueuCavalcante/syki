namespace Estud.Back.Features.Users.RegisterUser;

public class RegisterUserIn : IApiDto<RegisterUserIn>
{
    public string Email { get; set; }

    public static IEnumerable<(string, RegisterUserIn)> GetExamples() =>
    [
        ("Exemplo", new() { Email = "seu.email@gmail.com" })
    ];
}
