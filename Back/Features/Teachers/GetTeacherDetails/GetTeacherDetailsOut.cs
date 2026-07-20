namespace Estud.Back.Features.Teachers.GetTeacherDetails;

public class GetTeacherDetailsOut : IApiDto<GetTeacherDetailsOut>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<GetTeacherDetailsCampusOut> Campi { get; set; } = [];
    public List<GetTeacherDetailsDisciplineOut> Disciplines { get; set; } = [];
    public List<GetTeacherDetailsClassOut> Classes { get; set; } = [];

    public static IEnumerable<(string, GetTeacherDetailsOut)> GetExamples() =>
    [
        ("Exemplo", new GetTeacherDetailsOut
        {
            Id = 14,
            Name = "Ana Lima",
            Email = "ana.lima@estud.com",
            Campi = [new GetTeacherDetailsCampusOut { Id = 1, Name = "Campus Maceió" }],
            Disciplines = [new GetTeacherDetailsDisciplineOut { Id = 3, Name = "Banco de Dados" }],
            Classes =
            [
                new GetTeacherDetailsClassOut
                {
                    Id = 1,
                    Discipline = "Banco de Dados",
                    Period = "2026.1",
                    Vacancies = 40,
                    Students = 32,
                    Workload = 60,
                    Status = ClassStatus.Started,
                    Schedules = [new ScheduleOut(Day.Monday, Hour.H07_00, Hour.H10_00) { TeacherId = 14, Teacher = "Ana Lima" }],
                },
            ],
        }),
    ];
}

public class GetTeacherDetailsCampusOut
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class GetTeacherDetailsDisciplineOut
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class GetTeacherDetailsClassOut
{
    public int Id { get; set; }
    public string Discipline { get; set; }
    public string Period { get; set; }
    public int Vacancies { get; set; }

    /// <summary>
    /// Quantidade de alunos matriculados na turma
    /// </summary>
    public int Students { get; set; }

    public int Workload { get; set; }
    public ClassStatus Status { get; set; }

    /// <summary>
    /// Horários da turma cobertos por este professor
    /// </summary>
    public List<ScheduleOut> Schedules { get; set; } = [];
}
