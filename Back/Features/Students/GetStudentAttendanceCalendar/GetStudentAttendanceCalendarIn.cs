namespace Estud.Back.Features.Students.GetStudentAttendanceCalendar;

public class GetStudentAttendanceCalendarIn : IApiDto<GetStudentAttendanceCalendarIn>
{
    /// <summary>
    /// Ano do calendário. Quando não informado, usa o ano corrente.
    /// </summary>
    public int? Year { get; set; }

    public static IEnumerable<(string, GetStudentAttendanceCalendarIn)> GetExamples() =>
    [
        ("Exemplo", new GetStudentAttendanceCalendarIn { Year = 2026 }),
    ];
}
