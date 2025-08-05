namespace Syki.Shared;

public class UpdateEnrollmentPeriodIn
{
    public DateOnly StartAt { get; set; }
    public DateOnly EndAt { get; set; }

    public static IEnumerable<(string, UpdateEnrollmentPeriodIn)> GetExamples() =>
    [
        ("Novas datas", new() { StartAt = DateTime.Now.ToDateOnly(), EndAt = DateTime.Now.AddDays(30).ToDateOnly() }),
    ];
}
