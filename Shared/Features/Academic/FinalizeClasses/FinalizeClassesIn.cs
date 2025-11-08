namespace Syki.Shared;

public class FinalizeClassesIn : IApiDto<FinalizeClassesIn>
{
    public List<Guid> Classes { get; set; }

    public static IEnumerable<(string, FinalizeClassesIn)> GetExamples() =>
    [
        ("Única", new() { Classes = [Guid.CreateVersion7()]}),
        ("Múltiplas", new() { Classes = [Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7()]}),
    ];
}
