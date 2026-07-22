namespace Estud.Back.Features.Teachers.GetTeacherClass;

public class GetTeacherClassOut : IApiDto<GetTeacherClassOut>
{
    public int Id { get; set; }
    public string Discipline { get; set; }
    public string Period { get; set; }
    public int Vacancies { get; set; }
    public int Workload { get; set; }
    public ClassStatus Status { get; set; }
    public List<GetTeacherClassScheduleOut> Schedules { get; set; } = [];

    public static IEnumerable<(string, GetTeacherClassOut)> GetExamples() =>
    [
        ("Exemplo", new GetTeacherClassOut
        {
            Id = 1,
            Discipline = "Banco de Dados",
            Period = "2026.1",
            Vacancies = 40,
            Workload = 60,
            Status = ClassStatus.Started,
            Schedules = [new GetTeacherClassScheduleOut(Day.Monday, Hour.H07_00, Hour.H10_00) { Teacher = "Ana Lima", Classroom = "Sala 05" }],
        }),
    ];
}

public class GetTeacherClassScheduleOut
{
    public Day Day { get; set; }
    public Hour StartAt { get; set; }
    public Hour EndAt { get; set; }
    public string? Teacher { get; set; }
    public string? Classroom { get; set; }

    public GetTeacherClassScheduleOut() { }

    public GetTeacherClassScheduleOut(
        Day day,
        Hour startAt,
        Hour endAt
    ) {
        Day = day;
        StartAt = startAt;
        EndAt = endAt;
    }
}
