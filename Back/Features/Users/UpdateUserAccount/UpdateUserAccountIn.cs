namespace Estud.Back.Features.Users.UpdateUserAccount;

public class UpdateUserAccountIn : IApiDto<UpdateUserAccountIn>
{
    /// <summary>
    /// Nome do usuário.
    /// </summary>
    public string Name { get; set; }

    public static IEnumerable<(string, UpdateUserAccountIn)> GetExamples() =>
    [
        ("Exemplo", new UpdateUserAccountIn { Name = "Edson Gomes" }),
    ];
}
