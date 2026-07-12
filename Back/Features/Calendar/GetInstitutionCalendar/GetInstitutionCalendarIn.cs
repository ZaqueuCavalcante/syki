namespace Estud.Back.Features.Calendar.GetInstitutionCalendar;

public class GetInstitutionCalendarIn : IApiDto<GetInstitutionCalendarIn>
{
    /// <summary>
    /// Ano do calendário. Quando não informado, usa o ano corrente.
    /// </summary>
    public int? Year { get; set; }

    public static IEnumerable<(string, GetInstitutionCalendarIn)> GetExamples() =>
    [
        ("Exemplo", new GetInstitutionCalendarIn { Year = 2026 }),
    ];
}
