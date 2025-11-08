namespace Syki.Shared;

public class AddDisciplinePreRequisitesIn : IApiDto<AddDisciplinePreRequisitesIn>
{
    public Guid DisciplineId { get; set; }
    public List<Guid> PreRequisites { get; set; } = [];

    public static IEnumerable<(string, AddDisciplinePreRequisitesIn)> GetExamples() =>
    [
        ("Único", new() { DisciplineId = Guid.CreateVersion7(), PreRequisites = [Guid.CreateVersion7()]}),
        ("Múltiplos", new() { DisciplineId = Guid.CreateVersion7(), PreRequisites = [Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7()]}),
    ];
}
