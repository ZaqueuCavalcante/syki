namespace Estud.Back.Features.Periods.GetEnrollmentPeriods;

public class GetEnrollmentPeriodsOut : IApiDto<GetEnrollmentPeriodsOut>
{
    public int Total { get; set; }
    public List<GetEnrollmentPeriodsItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetEnrollmentPeriodsOut)> GetExamples() =>
    [
        ("Exemplo", new() { })
    ];
}

public class GetEnrollmentPeriodsItemOut
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateOnly StartAt { get; set; }

    public DateOnly EndAt { get; set; }
}
