namespace Syki.Shared;

public class CreateLessonAttendanceIn
{
    public List<Guid> PresentStudents { get; set; }

    public static IEnumerable<(string, CreateLessonAttendanceIn)> GetExamples() =>
    [
        ("Único", new() { PresentStudents = [Guid.CreateVersion7()]}),
        ("Múltiplos", new() { PresentStudents = [Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7()]}),
    ];
}
