namespace Exato.Shared.Features.Office.UpdateRole;

public class UpdateRoleIn : IApiDto<UpdateRoleIn>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<int> Features { get; set; } = [];

    public static IEnumerable<(string Name, UpdateRoleIn Value)> GetExamples() =>
    [
        ("Exemplo",
        new UpdateRoleIn
        {
            Name = "Recrutador",
            Description = "Recrutador de pessoas altamente capacitadas.",
            Features = [1, 5, 39, 45],
        }),
    ];
}
