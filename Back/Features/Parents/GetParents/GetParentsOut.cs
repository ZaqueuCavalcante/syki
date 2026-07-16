namespace Estud.Back.Features.Parents.GetParents;

public class GetParentsOut : IApiDto<GetParentsOut>
{
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<GetParentsItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetParentsOut)> GetExamples() =>
    [
        ("Exemplo", new()
        {
            Total = 1,
            Page = 1,
            PageSize = 10,
            Items = [new() { Id = 1, Name = "Ana Souza", Email = "ana.souza@gmail.com", PhoneNumber = "82988887777", Students = ["Maria Souza"] }],
        }),
    ];
}

public class GetParentsItemOut
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public List<string> Students { get; set; } = [];
}
