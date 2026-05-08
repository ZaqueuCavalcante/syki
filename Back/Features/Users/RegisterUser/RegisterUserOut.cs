namespace Syki.Back.Features.Users.RegisterUser;

public class RegisterUserOut : IApiDto<RegisterUserOut>
{
    public Guid Id { get; set; }

    public static IEnumerable<(string, RegisterUserOut)> GetExamples() =>
    [
        ("Exemplo", new RegisterUserOut { Id = Guid.NewGuid() }),
    ];
}
