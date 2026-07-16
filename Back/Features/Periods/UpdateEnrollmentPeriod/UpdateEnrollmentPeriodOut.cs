namespace Estud.Back.Features.Periods.UpdateEnrollmentPeriod;

public class UpdateEnrollmentPeriodOut : IApiDto<UpdateEnrollmentPeriodOut>
{
    public int Id { get; set; }

    public string Name { get; set; }

    /// <summary>
    /// Data de início
    /// </summary>
    public DateOnly StartAt { get; set; }

    /// <summary>
    /// Data de término
    /// </summary>
    public DateOnly EndAt { get; set; }

    public static IEnumerable<(string, UpdateEnrollmentPeriodOut)> GetExamples() =>
    [
        ("Exemplo", new() { Id = 1, Name = "Matrículas 2024.1", StartAt = new(2024, 01, 15), EndAt = new(2024, 02, 01) }),
    ];
}
