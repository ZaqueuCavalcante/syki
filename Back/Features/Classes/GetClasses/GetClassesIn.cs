namespace Estud.Back.Features.Classes.GetClasses;

public class GetAcademicClassesIn : IApiDto<GetAcademicClassesIn>
{
    public ClassStatus? Status { get; set; }
    public bool? AllLessonsFinished { get; set; }

    public static IEnumerable<(string, GetAcademicClassesIn)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}
