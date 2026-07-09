namespace Estud.Back.Features.Identity.CreateRole;

public class CreateRoleOut : IApiDto<CreateRoleOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, CreateRoleOut)> GetExamples() =>
    [
        ("Exemplo", new CreateRoleOut { Id = 1 }),
    ];
}
