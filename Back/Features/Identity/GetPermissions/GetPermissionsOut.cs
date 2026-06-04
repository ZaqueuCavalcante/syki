namespace Syki.Back.Features.Identity.GetPermissions;

public class GetPermissionsOut : IApiDto<GetPermissionsOut>
{
    public List<GetPermissionsItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetPermissionsOut)> GetExamples() =>
    [
        ("Exemplo", new()
        {
            Items =
            [
                new() { Id = 0, Name = "Gerenciar perfis de acesso.", AllowedTypes = [UserType.Manager] },
                new() { Id = 100, Name = "Gerenciar usuários.", AllowedTypes = [UserType.Manager] },
            ],
        }),
    ];
}

public class GetPermissionsItemOut
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<UserType> AllowedTypes { get; set; }
}
