namespace Syki.Shared;

public class SuccessOut : IApiDto<SuccessOut>
{
    public static IEnumerable<(string, SuccessOut)> GetExamples() =>
    [
        ("Success", new()),
    ];
}
