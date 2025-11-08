namespace Exato.Shared.Features.Office.CreateUser;

public class CreateUserOut : IApiDto<CreateUserOut>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int OrganizationId { get; set; }
    public string Role { get; set; }

    public static IEnumerable<(string, CreateUserOut)> GetExamples() =>
    [
        ("Exemplo",
        new CreateUserOut
        {
            Id = Guid.NewGuid(),
            Name = "User",
            Email = "user@exato.com",
            OrganizationId = 354684,
            Role = "OrgRecruiter",
        }),
    ];
}
