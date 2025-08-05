namespace Syki.Shared;

public class GetAcademicClassesIn
{
    public ClassStatus? Status { get; set; }
    public bool? AllLessonsFinished { get; set; }

    public static IEnumerable<(string, GetAcademicClassesIn)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}
