namespace Estud.Back.Features.Classes.GetClasses;

public class GetClassesOut : IApiDto<GetClassesOut>
{
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<GetClassesItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetClassesOut)> GetExamples() =>
    [
        ("Exemplo", new GetClassesOut
        {
            Total = 1,
            Page = 1,
            PageSize = 10,
            Items = [new GetClassesItemOut { Id = 1, Discipline = "Banco de Dados", Period = "2026.1", Vacancies = 40, Status = ClassStatus.OnEnrollment }]
        })
    ];
}

public class GetClassesItemOut
{
    public int Id { get; set; }
    public string Discipline { get; set; }
    public string Period { get; set; }
    public int Vacancies { get; set; }
    public ClassStatus Status { get; set; }
    public List<string> Teachers { get; set; } = [];
    public List<GetClassesScheduleOut> Schedules { get; set; } = [];
}

public class GetClassesScheduleOut
{
    public Day Day { get; set; }
    public Hour StartAt { get; set; }
    public Hour EndAt { get; set; }

    public int? TeacherId { get; set; }
    public string? Teacher { get; set; }

    public int? ClassroomId { get; set; }
    public string? Classroom { get; set; }

    public GetClassesScheduleOut() { }

    public GetClassesScheduleOut(
        Day day,
        Hour startAt,
        Hour endAt
    ) {
        Day = day;
        StartAt = startAt;
        EndAt = endAt;
    }
}
