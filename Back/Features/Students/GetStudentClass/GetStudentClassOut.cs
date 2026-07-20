namespace Estud.Back.Features.Students.GetStudentClass;

public class GetStudentClassOut : IApiDto<GetStudentClassOut>
{
    public int Id { get; set; }
    public string Discipline { get; set; }
    public string Period { get; set; }
    public int Workload { get; set; }
    public ClassStatus Status { get; set; }
    public StudentClassStatus MyStatus { get; set; }
    public List<string> Teachers { get; set; } = [];
    public List<GetStudentClassScheduleOut> Schedules { get; set; } = [];

    public static IEnumerable<(string, GetStudentClassOut)> GetExamples() =>
    [
        ("Exemplo", new GetStudentClassOut
        {
            Id = 1,
            Discipline = "Banco de Dados",
            Period = "2026.1",
            Workload = 60,
            Status = ClassStatus.Started,
            MyStatus = StudentClassStatus.Matriculado,
            Teachers = ["Chico Ferreira", "Ana Lima"],
            Schedules = [new GetStudentClassScheduleOut(Day.Monday, Hour.H07_00, Hour.H10_00)],
        }),
    ];
}

public class GetStudentClassScheduleOut
{
    public Day Day { get; set; }
    public Hour StartAt { get; set; }
    public Hour EndAt { get; set; }

    public int? TeacherId { get; set; }
    public string? Teacher { get; set; }

    public int? ClassroomId { get; set; }
    public string? Classroom { get; set; }

    public GetStudentClassScheduleOut() { }

    public GetStudentClassScheduleOut(
        Day day,
        Hour startAt,
        Hour endAt
    ) {
        Day = day;
        StartAt = startAt;
        EndAt = endAt;
    }
}
