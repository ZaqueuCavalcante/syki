namespace Syki.Shared;

public class AssignCampiToTeacherIn
{
    public List<Guid> Campi { get; set; }

    public static IEnumerable<(string, AssignCampiToTeacherIn)> GetExamples() =>
    [
        ("Única", new() { Campi = [Guid.CreateVersion7()]}),
        ("Múltiplas", new() { Campi = [Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7()]}),
    ];
}
