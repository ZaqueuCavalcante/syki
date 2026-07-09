namespace Estud.Back.Features.Identity.CreateRole;

public class CreateRoleIn : IApiDto<CreateRoleIn>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public UserType BaseType { get; set; }
    public List<int> Permissions { get; set; } = [];

    public static IEnumerable<(string, CreateRoleIn)> GetExamples() =>
    [
        ("Exemplo", new CreateRoleIn
        {
            Name = "Admin",
            Description = "Administrador com acesso total",
            BaseType = UserType.Manager,
            Permissions = [1, 2, 3],
        }),
    ];
}
