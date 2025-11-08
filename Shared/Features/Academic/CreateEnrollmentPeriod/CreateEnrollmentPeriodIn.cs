namespace Syki.Shared;

public class CreateEnrollmentPeriodIn : IApiDto<CreateEnrollmentPeriodIn>
{
    public string Id { get; set; }
    public DateOnly StartAt { get; set; }
    public DateOnly EndAt { get; set; }

    public static IEnumerable<(string, CreateEnrollmentPeriodIn)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}
