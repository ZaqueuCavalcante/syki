namespace Syki.Shared;

public class AssignClassToClassroomIn
{
    public Guid ClassId { get; set; }
    public List<ScheduleIn> Schedules { get; set; }

    public static IEnumerable<(string, AssignClassToClassroomIn)> GetExamples() =>
    [
        ("Exemplo", new() { ClassId = Guid.CreateVersion7() }),
    ];
}
