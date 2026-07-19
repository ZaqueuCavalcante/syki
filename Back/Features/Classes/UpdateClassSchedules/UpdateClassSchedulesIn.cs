namespace Estud.Back.Features.Classes.UpdateClassSchedules;

public class UpdateClassSchedulesIn : IApiDto<UpdateClassSchedulesIn>
{
    public List<UpdateClassScheduleIn> Schedules { get; set; } = [];

    public static IEnumerable<(string, UpdateClassSchedulesIn)> GetExamples() =>
    [
        ("Exemplo", new()
        {
            Schedules =
            [
                new() { Day = Day.Monday, Start = Hour.H07_00, End = Hour.H10_00, TeacherId = 14 },
                new() { Day = Day.Wednesday, Start = Hour.H07_00, End = Hour.H10_00, TeacherId = 32 },
            ],
        }),
    ];
}

public class UpdateClassScheduleIn
{
    public Day Day { get; set; }
    public Hour Start { get; set; }
    public Hour End { get; set; }

    /// <summary>
    /// Professor que cobre este horário.
    /// Obrigatório quando a turma tem 2 professores.
    /// Com 1 professor é preenchido automaticamente.
    /// Ignorado quando a turma não tem professores.
    /// </summary>
    public int? TeacherId { get; set; }
}
