namespace Estud.Back.Features.Calendar.GetInstitutionCalendar;

public class GetInstitutionCalendarOut : IApiDto<GetInstitutionCalendarOut>
{
    public int Year { get; set; }
    public int Total { get; set; }
    public List<GetInstitutionCalendarItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetInstitutionCalendarOut)> GetExamples() =>
    [
        ("Exemplo", new GetInstitutionCalendarOut
        {
            Year = 2026,
            Total = 2,
            Items =
            [
                new() { Date = new DateTime(2026, 1, 1), DayType = DayType.Holiday, Description = "Confraternização Universal" },
                new() { Id = 1, Date = new DateTime(2026, 1, 2), DayType = DayType.Vacation, Description = "Férias de verão" },
            ]
        }),
    ];
}

public class GetInstitutionCalendarItemOut
{
    /// <summary>
    /// Id do dia customizado. Nulo quando o dia não foi customizado pela instituição.
    /// </summary>
    public int? Id { get; set; }

    public DateTime Date { get; set; }

    public DayType DayType { get; set; }

    public string? Description { get; set; }
}
