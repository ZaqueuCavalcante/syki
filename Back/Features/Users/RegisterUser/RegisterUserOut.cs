namespace Syki.Back.Features.Users.RegisterUser;

public class RegisterUserOut : IApiDto<RegisterUserOut>
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }

    public static IEnumerable<(string, RegisterUserOut)> GetExamples() =>
    [
        ("Exemplo", new RegisterUserOut { Id = 1, InstitutionId = 1 }),
    ];
}
