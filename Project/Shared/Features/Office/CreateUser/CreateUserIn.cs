namespace Exato.Shared.Features.Office.CreateUser;

public class CreateUserIn : IApiDto<CreateUserIn>
{
    public int OrganizationId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Guid RoleId { get; set; }

    public static IEnumerable<(string Name, CreateUserIn Value)> GetExamples() =>
    [
        ("Exemplo",
        new CreateUserIn
        {
            Name = "User",
            Email = "user@exato.com",
            OrganizationId = 852456,
            RoleId = Guid.NewGuid(),
        }),
    ];
}
