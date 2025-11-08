namespace Exato.Shared.Features.Cross.GetUserAccount;

public class GetUserAccountOut : IApiDto<GetUserAccountOut>
{
    /// <summary>
    /// Id do usuário.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nome do usuário.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Email do usuário.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Role do usuário.
    /// </summary>
    public string Role { get; set; }

    /// <summary>
    /// Features do usuário.
    /// </summary>
    public List<int> Features { get; set; } = [];

    /// <summary>
    /// Organização do usuário.
    /// </summary>
    public string Organization { get; set; }

    public static IEnumerable<(string, GetUserAccountOut)> GetExamples() =>
    [
        ("Zaqueu Cavalcante",
        new GetUserAccountOut()
        {
            Id = Guid.CreateVersion7(),
            Name = "Zaqueu Cavalcante",
            Email = "zaqueu.cavalcante@exato.com",
            Role = "OrgAdm",
            Features = [1, 2, 3],
            Organization = "Exato Digital",
        }),
        ("Maria Júlia",
        new GetUserAccountOut()
        {
            Id = Guid.CreateVersion7(),
            Name = "Maria Júlia",
            Email = "maria.julia@exato.com",
            Role = "OrgRecruiter",
            Features = [80, 81],
            Organization = "Exato Digital",
        }),
    ];
}
