namespace Exato.Shared.Features.Cross.Login;

public class LoginOut : IApiDto<LoginOut>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public List<int> Features { get; set; } = [];

    public static IEnumerable<(string, LoginOut)> GetExamples() =>
    [
        ("Exemplo",
        new LoginOut
        {
            Id = Guid.NewGuid(),
            Name = "User",
            Email = "user@exato.com",
            Role = "OrgRecruiter",
            Features = [1, 2, 3],
        }),
    ];
}
