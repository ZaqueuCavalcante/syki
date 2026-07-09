namespace Estud.Back.Features.Periods.CreateEnrollmentPeriod;

public class CreateEnrollmentPeriodOut : IApiDto<CreateEnrollmentPeriodOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, CreateEnrollmentPeriodOut)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}
