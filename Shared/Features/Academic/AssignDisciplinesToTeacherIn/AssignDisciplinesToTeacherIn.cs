namespace Syki.Shared;

public class AssignDisciplinesToTeacherIn : IApiDto<AssignDisciplinesToTeacherIn>
{
    public List<Guid> Disciplines { get; set; }

    public static IEnumerable<(string, AssignDisciplinesToTeacherIn)> GetExamples() =>
    [
        ("Única", new() { Disciplines = [Guid.CreateVersion7()]}),
        ("Múltiplas", new() { Disciplines = [Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7()]}),
    ];
}
