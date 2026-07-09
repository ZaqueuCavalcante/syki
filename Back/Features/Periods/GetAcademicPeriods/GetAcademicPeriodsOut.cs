namespace Estud.Back.Features.Periods.GetAcademicPeriods;

public class GetAcademicPeriodsOut : IApiDto<GetAcademicPeriodsOut>
{
    public int Total { get; set; }
    public List<GetAcademicPeriodsItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetAcademicPeriodsOut)> GetExamples() =>
    [
        ("2024.1", new() { })
    ];
}

public class GetAcademicPeriodsItemOut
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateOnly StartAt { get; set; }

    public DateOnly EndAt { get; set; }
}
