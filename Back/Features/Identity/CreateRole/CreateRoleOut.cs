namespace Syki.Back.Features.Identity.CreateRole;

public class CreateRoleOut : IApiDto<CreateRoleOut>
{
    public Guid Id { get; set; }

    public static IEnumerable<(string, CreateRoleOut)> GetExamples() =>
    [
        ("Exemplo", new CreateRoleOut { Id = Guid.NewGuid() }),
    ];
}
