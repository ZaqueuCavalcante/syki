namespace Syki.Shared;

public class AddDisciplinePreRequisitesIn
{
    public List<Guid> PreRequisites { get; set; } = [];

    public static IEnumerable<(string, AddDisciplinePreRequisitesIn)> GetExamples() =>
    [
        ("Único", new() { PreRequisites = [Guid.CreateVersion7()]}),
        ("Múltiplos", new() { PreRequisites = [Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7()]}),
    ];
}
