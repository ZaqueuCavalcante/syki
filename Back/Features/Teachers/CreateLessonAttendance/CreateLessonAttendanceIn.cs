namespace Estud.Back.Features.Teachers.CreateLessonAttendance;

public class CreateLessonAttendanceIn : IApiDto<CreateLessonAttendanceIn>
{
    /// <summary>
    /// Ids dos alunos presentes na aula
    /// </summary>
    public List<int> PresentStudents { get; set; } = [];

    public static IEnumerable<(string, CreateLessonAttendanceIn)> GetExamples() =>
    [
        ("Exemplo", new CreateLessonAttendanceIn { PresentStudents = [1, 2, 3] }),
    ];
}
