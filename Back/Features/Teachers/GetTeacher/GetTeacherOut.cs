namespace Syki.Back.Features.Teachers.GetTeacher;

public class GetTeacherOut : IApiDto<GetTeacherOut>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<GetTeacherCampusOut> Campi { get; set; } = [];
    public List<GetTeacherDisciplineOut> Disciplines { get; set; } = [];

    public static IEnumerable<(string, GetTeacherOut)> GetExamples() =>
    [
        ("Exemplo", new()
        {
            Id = 1,
            Name = "João Silva",
            Email = "joao@ufal.edu.br",
            Campi = [new() { Id = 1, Name = "Campus A" }],
            Disciplines = [new() { Id = 1, Name = "Cálculo I" }],
        }),
    ];
}

public class GetTeacherCampusOut
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class GetTeacherDisciplineOut
{
    public int Id { get; set; }
    public string Name { get; set; }
}
