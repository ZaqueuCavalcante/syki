namespace Estud.Back.Features.Teachers.AssignCampiToTeacher;

public class AssignCampiToTeacherIn : IApiDto<AssignCampiToTeacherIn>
{
    public List<int> Campi { get; set; } = [];

    public static IEnumerable<(string, AssignCampiToTeacherIn)> GetExamples() =>
    [
        ("Exemplo", new() { Campi = [1, 2] }),
    ];
}
