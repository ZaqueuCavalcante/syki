namespace Syki.Shared;

public class ReleaseClassesForEnrollmentIn : IApiDto<ReleaseClassesForEnrollmentIn>
{
    public List<Guid> Classes { get; set; }

    public static IEnumerable<(string, ReleaseClassesForEnrollmentIn)> GetExamples() =>
    [
        ("Única", new() { Classes = [Guid.CreateVersion7()]}),
        ("Múltiplas", new() { Classes = [Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7()]}),
    ];
}
