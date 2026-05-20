namespace Syki.Back.Features.Users.GetUserAccount;

public class GetUserAccountOut : IApiDto<GetUserAccountOut>
{
    /// <summary>
    /// Id do usuário.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome do usuário.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Email do usuário.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Instituição do usuário.
    /// </summary>
    public string Institution { get; set; }

    /// <summary>
    /// Role do usuário.
    /// </summary>
    public string Role { get; set; }

    /// <summary>
    /// Url da foto de perfil do usuário.
    /// </summary>
    public string ProfilePhoto { get; set; }

    /// <summary>
    /// Curso, caso seja um Aluno.
    /// </summary>
    public string? Course { get; set; }

    public static IEnumerable<(string, GetUserAccountOut)> GetExamples() =>
    [
        ("Edson Gomes",
        new GetUserAccountOut()
        {
            Id = 1,
            Name = "Edson Gomes",
            Email = "edson.gomes@syki.com.br",
            Institution = "UFPE",
        }),
        ("Maria Júlia",
        new GetUserAccountOut()
        {
            Id = 2,
            Name = "Maria Júlia",
            Email = "maria.julia@syki.com.br",
            Institution = "Faculdade Nova Roma",
        }),
    ];
}
