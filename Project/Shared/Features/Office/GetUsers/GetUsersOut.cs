namespace Exato.Shared.Features.Office.GetUsers;

public class GetUsersOut : IApiDto<GetUsersOut>
{
    public int Total { get; set; }
    public List<GetUsersItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetUsersOut)> GetExamples() =>
    [
        ("Exemplo", new GetUsersOut() { }),
    ];
}

public class GetUsersItemOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool TwoFactorEnabled { get; set; }
}
