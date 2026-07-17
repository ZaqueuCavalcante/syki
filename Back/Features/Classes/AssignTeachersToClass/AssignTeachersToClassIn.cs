namespace Estud.Back.Features.Classes.AssignTeachersToClass;

public class AssignTeachersToClassIn : IApiDto<AssignTeachersToClassIn>
{
    public List<int> Teachers { get; set; } = [];

    public static IEnumerable<(string, AssignTeachersToClassIn)> GetExamples() =>
    [
        ("Exemplo", new() { Teachers = [14, 32] }),
    ];
}
