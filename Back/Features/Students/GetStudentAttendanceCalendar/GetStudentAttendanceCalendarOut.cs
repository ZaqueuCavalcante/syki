namespace Estud.Back.Features.Students.GetStudentAttendanceCalendar;

public class GetStudentAttendanceCalendarOut : IApiDto<GetStudentAttendanceCalendarOut>
{
    public int Year { get; set; }
    public int Total { get; set; }
    public List<GetStudentAttendanceCalendarItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetStudentAttendanceCalendarOut)> GetExamples() =>
    [
        ("Exemplo", new GetStudentAttendanceCalendarOut
        {
            Year = 2026,
            Total = 4,
            Items =
            [
                new() { Date = new DateTime(2026, 3, 7), Status = StudentDayAttendanceStatus.NoClass },
                new() { Date = new DateTime(2026, 3, 9), Status = StudentDayAttendanceStatus.Present },
                new() { Date = new DateTime(2026, 3, 11), Status = StudentDayAttendanceStatus.Absent },
                new() { Date = new DateTime(2026, 3, 13), Status = StudentDayAttendanceStatus.Undefined },
            ]
        }),
    ];
}

public class GetStudentAttendanceCalendarItemOut
{
    public DateTime Date { get; set; }

    /// <summary>
    /// Status de frequência do aluno no dia:
    /// 0 = sem aula (fim de semana, feriado, férias, recesso ou dia sem aula do aluno);
    /// 1 = indefinido (aula futura ou frequência ainda não lançada);
    /// 2 = presença; 3 = falta.
    /// </summary>
    public StudentDayAttendanceStatus Status { get; set; }
}
