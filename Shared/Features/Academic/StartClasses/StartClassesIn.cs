namespace Syki.Shared;

public class StartClassesIn : IApiDto<StartClassesIn>
{
    public List<Guid> Classes { get; set; }

    public static IEnumerable<(string, StartClassesIn)> GetExamples() =>
    [
        ("Única", new() { Classes = [Guid.CreateVersion7()]}),
        ("Múltiplas", new() { Classes = [Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7()]}),
    ];
}
