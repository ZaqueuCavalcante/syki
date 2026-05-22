namespace Syki.Back.Features.Identity.GetRoles;

public class GetRolesOut : IApiDto<GetRolesOut>
{
    public int Total { get; set; }
    public List<GetRolesItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetRolesOut)> GetExamples() =>
    [
        ("Exemplo", new() { Total = 1, Items = [new() { Id = 1, Name = "Admin", Description = "Perfil de administrador", Permissions = 5 }] }),
    ];
}

public class GetRolesItemOut
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Permissions { get; set; }
}
