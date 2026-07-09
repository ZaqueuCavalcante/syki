namespace Estud.Back.Features.Identity.UpdateRole;

public class UpdateRoleIn : IApiDto<UpdateRoleIn>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public UserType BaseType { get; set; }
    public List<int> Permissions { get; set; } = [];

    public static IEnumerable<(string, UpdateRoleIn)> GetExamples() =>
    [
        ("Exemplo", new() { Id = 1, Name = "Admin", Description = "Perfil de administrador", BaseType = UserType.Manager, Permissions = [1, 2, 3] }),
    ];
}
