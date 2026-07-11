namespace Estud.Back.Features.Students.AssignStudentToClass;

public class AssignStudentToClassIn : IApiDto<AssignStudentToClassIn>
{
    public int ClassId { get; set; }

    public static IEnumerable<(string, AssignStudentToClassIn)> GetExamples() =>
    [
        ("Exemplo", new() { ClassId = 1 }),
    ];
}
