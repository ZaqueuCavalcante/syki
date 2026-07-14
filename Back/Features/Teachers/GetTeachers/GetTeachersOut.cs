namespace Estud.Back.Features.Teachers.GetTeachers;

public class GetTeachersOut : IApiDto<GetTeachersOut>
{
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<GetTeachersItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetTeachersOut)> GetExamples() =>
    [
        ("Exemplo", new() { Total = 1, Page = 1, PageSize = 10, Items = [new() { Id = 1, Name = "João Silva", Email = "joao@ufal.edu.br", Campi = 1, Disciplines = 3 }] }),
    ];
}

public class GetTeachersItemOut
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int Campi { get; set; }
    public int Disciplines { get; set; }
}
