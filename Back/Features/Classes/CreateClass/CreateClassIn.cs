namespace Syki.Back.Features.Classes.CreateClass;

public class CreateClassIn : IApiDto<CreateClassIn>
{
    public int DisciplineId { get; set; }
    public int? CampusId { get; set; }
    public int? TeacherId { get; set; }
    public int PeriodId { get; set; }
    public int Vacancies { get; set; }
    public List<CreateClassScheduleIn> Schedules { get; set; }

    public static IEnumerable<(string, CreateClassIn)> GetExamples() =>
    [
        ("Banco de Dados",
        new CreateClassIn
        {
            DisciplineId = 1,
            CampusId = 1,
            TeacherId = 1,
            PeriodId = 2,
            Vacancies = 40,
            Schedules = [
                new CreateClassScheduleIn { Day = Day.Monday, Start = Hour.H07_00, End = Hour.H10_00 },
                new CreateClassScheduleIn { Day = Day.Thursday, Start = Hour.H08_00, End = Hour.H10_30 },
            ]
        }),
    ];
}

public class CreateClassScheduleIn
{
    public Day Day { get; set; }
    public Hour Start { get; set; }
    public Hour End { get; set; }
}
