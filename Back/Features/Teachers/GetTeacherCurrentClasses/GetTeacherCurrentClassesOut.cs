namespace Estud.Back.Features.Teachers.GetTeacherCurrentClasses;

public class GetTeacherCurrentClassesOut : IApiDto<GetTeacherCurrentClassesOut>
{
    public List<GetTeacherCurrentClassesItemOut> Classes { get; set; } = [];

    public static IEnumerable<(string Name, GetTeacherCurrentClassesOut Value)> GetExamples() =>
    [
        new() { Name = "Exemplo", Value = new()
        {
            Classes =
            [
                new() { Id = 1, Name = "Cálculo I" },
                new() { Id = 2, Name = "Geometria Analítica" },
            ]
        }}
    ];
}

public class GetTeacherCurrentClassesItemOut
{
    public int Id { get; set; }
    public string Name { get; set; }
}
