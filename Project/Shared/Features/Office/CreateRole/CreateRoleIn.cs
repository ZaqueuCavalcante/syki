namespace Exato.Shared.Features.Office.CreateRole;

public class CreateRoleIn : IApiDto<CreateRoleIn>
{
    public int OrganizationId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<int> Features { get; set; } = [];

    public static IEnumerable<(string Name, CreateRoleIn Value)> GetExamples() =>
    [
        ("Exemplo",
        new CreateRoleIn
        {
            Name = "Recrutador",
            Description = "Recrutador de pessoas capacitadas.",
            OrganizationId = 852456,
            Features = [1, 5, 39],
        }),
    ];
}
