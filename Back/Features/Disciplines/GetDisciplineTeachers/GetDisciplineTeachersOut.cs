namespace Estud.Back.Features.Disciplines.GetDisciplineTeachers;

public class GetDisciplineTeachersOut : IApiDto<GetDisciplineTeachersOut>
{
    public List<GetDisciplineTeacherItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetDisciplineTeachersOut)> GetExamples() =>
    [
        ("Exemplo", new()
        {
            Items =
            [
                new() { Id = 14, Name = "Ana Lima" },
                new() { Id = 32, Name = "Chico Ferreira" },
            ]
        }),
    ];
}

public class GetDisciplineTeacherItemOut
{
    public int Id { get; set; }
    public string Name { get; set; }
}
