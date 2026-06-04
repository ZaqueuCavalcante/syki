namespace Syki.Back.Features.Classes.GetClasses;

public class GetClassesOut : IApiDto<GetClassesOut>
{
    public int Total { get; set; }
    public List<GetClassesItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetClassesOut)> GetExamples() =>
    [
        ("Exemplo", new GetClassesOut
        {
            Total = 1,
            Items = [new GetClassesItemOut { Id = 1, Discipline = "Banco de Dados", Period = "2026.1", Vacancies = 40, Status = ClassStatus.OnEnrollment }]
        })
    ];
}

public class GetClassesItemOut
{
    public int Id { get; set; }
    public string Discipline { get; set; }
    public string Teacher { get; set; }
    public string Period { get; set; }
    public int Vacancies { get; set; }
    public ClassStatus Status { get; set; }
    public List<ScheduleOut> Schedules { get; set; } = [];
}
