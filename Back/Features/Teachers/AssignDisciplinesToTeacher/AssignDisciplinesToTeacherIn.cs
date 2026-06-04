namespace Syki.Back.Features.Teachers.AssignDisciplinesToTeacher;

public class AssignDisciplinesToTeacherIn : IApiDto<AssignDisciplinesToTeacherIn>
{
    public List<int> Disciplines { get; set; } = [];

    public static IEnumerable<(string, AssignDisciplinesToTeacherIn)> GetExamples() =>
    [
        ("Exemplo", new() { Disciplines = [1, 2, 3] }),
    ];
}
