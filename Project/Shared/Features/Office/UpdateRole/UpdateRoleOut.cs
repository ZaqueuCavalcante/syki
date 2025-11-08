namespace Exato.Shared.Features.Office.UpdateRole;

public class UpdateRoleOut : IApiDto<UpdateRoleOut>
{
    public Guid Id { get; set; }

    public static IEnumerable<(string, UpdateRoleOut)> GetExamples() =>
    [
        ("Exemplo",
        new UpdateRoleOut
        {
            Id = Guid.NewGuid(),
        }),
    ];
}
