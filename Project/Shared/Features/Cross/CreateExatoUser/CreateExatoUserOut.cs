namespace Exato.Shared.Features.Cross.CreateExatoUser;

public class CreateExatoUserOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public int OrganizationId { get; set; }

    public static IEnumerable<(string, CreateExatoUserOut)> GetExamples() =>
    [
        ("Exemplo",
        new CreateExatoUserOut
        {
            Id = Guid.NewGuid(),
            Name = "User",
            Email = "user@exato.com",
            OrganizationId = 951753,
            Role = "OrgRecruiter",
        }),
    ];
}
