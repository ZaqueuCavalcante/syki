namespace Estud.Back.Features.Identity.GetTwoFactorEnforcement;

public class GetTwoFactorEnforcementOut : IApiDto<GetTwoFactorEnforcementOut>
{
    public int Total { get; set; }
    public List<GetTwoFactorEnforcementItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetTwoFactorEnforcementOut)> GetExamples() =>
    [
        ("Exemplo", new()
        {
            Total = 2,
            Items =
            [
                new() { RoleId = 1, Name = "Diretor", BaseType = UserType.Manager, TwoFactorRequired = true },
                new() { RoleId = 2, Name = "Aluno", BaseType = UserType.Student, TwoFactorRequired = false },
            ],
        }),
    ];
}

public class GetTwoFactorEnforcementItemOut
{
    public int RoleId { get; set; }
    public string Name { get; set; }
    public UserType BaseType { get; set; }
    public bool TwoFactorRequired { get; set; }
}
