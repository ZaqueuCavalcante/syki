namespace Syki.Shared;

public class EnrollmentPeriodOut
{
    public string Id { get; set; }
    public DateOnly StartAt { get; set; }
    public DateOnly EndAt { get; set; }

    public static IEnumerable<(string, EnrollmentPeriodOut)> GetExamples() =>
    [
        ("Período de matrícula", new() { Id = $"{DateTime.Now.Year}.1", StartAt = DateTime.Now.ToDateOnly(), EndAt = DateTime.Now.AddDays(30).ToDateOnly() }),
    ];

    public static implicit operator EnrollmentPeriodOut(OneOf<EnrollmentPeriodOut, ErrorOut> value)
    {
        return value.Success;
    }
}
