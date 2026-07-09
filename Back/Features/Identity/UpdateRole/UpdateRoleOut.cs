namespace Estud.Back.Features.Identity.UpdateRole;

public class UpdateRoleOut : IApiDto<UpdateRoleOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, UpdateRoleOut)> GetExamples() =>
    [
        ("Exemplo", new() { Id = 1 }),
    ];
}
