namespace Estud.Back.Features.Periods.CreateEnrollmentPeriod;

public class CreateEnrollmentPeriodIn : IApiDto<CreateEnrollmentPeriodIn>
{
    public string? Name { get; set; }

    /// <summary>
    /// Data de início
    /// </summary>
    public DateOnly StartAt { get; set; }

    /// <summary>
    /// Data de término
    /// </summary>
    public DateOnly EndAt { get; set; }

    public static IEnumerable<(string, CreateEnrollmentPeriodIn)> GetExamples() =>
    [
        ("Exemplo", new() { Name = "2024.1", StartAt = new(2024, 01, 15), EndAt = new(2024, 02, 01) }),
    ];
}
