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
    public List<ScheduleOut> Schedules { get; set; } = [];

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
            Schedules = [new ScheduleOut(Day.Monday, Hour.H07_00, Hour.H10_00)],
        }),
    ];
}
