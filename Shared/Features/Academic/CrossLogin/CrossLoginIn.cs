namespace Syki.Shared;

public class CrossLoginIn
{
    public Guid TargetUserId { get; set; }

    public static IEnumerable<(string, CrossLoginIn)> GetExamples() =>
    [
        ("Aluno", new() { TargetUserId = Guid.CreateVersion7() }),
        ("Professor", new() { TargetUserId = Guid.CreateVersion7() }),
    ];
}
