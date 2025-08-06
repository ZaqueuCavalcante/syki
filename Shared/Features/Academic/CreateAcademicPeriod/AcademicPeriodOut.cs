namespace Syki.Shared;

public class AcademicPeriodOut
{
    public string Id { get; set; }

    /// <summary>
    /// Data de início
    /// </summary>
    public DateOnly StartAt { get; set; }

    /// <summary>
    /// Data de término
    /// </summary>
    public DateOnly EndAt { get; set; }

    public static IEnumerable<(string, AcademicPeriodOut)> GetExamples() =>
    [
        ("2024.1", new() { Id = "2024.1", StartAt = new(2024, 02, 01), EndAt = new(2024, 06, 05) }),
        ("2024.2", new() { Id = "2024.2", StartAt = new(2024, 07, 08), EndAt = new(2024, 12, 10) }),
    ];

    public static implicit operator AcademicPeriodOut(OneOf<AcademicPeriodOut, ErrorOut> value)
    {
        return value.Success;
    }
}
