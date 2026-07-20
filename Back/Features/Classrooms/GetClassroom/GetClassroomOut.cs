namespace Estud.Back.Features.Classrooms.GetClassroom;

public class GetClassroomOut : IApiDto<GetClassroomOut>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CampusId { get; set; }
    public string Campus { get; set; }
    public int Capacity { get; set; }

    /// <summary>
    /// Quantidade de turmas distintas alocadas na sala
    /// </summary>
    public int ClassesCount { get; set; }

    /// <summary>
    /// Total de horas por semana em que a sala está ocupada
    /// </summary>
    public decimal WeeklyHours { get; set; }

    /// <summary>
    /// Maior quantidade de alunos entre as turmas alocadas na sala. Serve para
    /// comparar com a capacidade e identificar salas sobrecarregadas.
    /// </summary>
    public int PeakStudents { get; set; }

    /// <summary>
    /// Agenda da sala: turmas alocadas com seus dias e horários.
    /// </summary>
    public List<ClassroomScheduleOut> Schedules { get; set; } = [];

    public static IEnumerable<(string, GetClassroomOut)> GetExamples() =>
    [
        ("Exemplo", new GetClassroomOut
        {
            Id = 1,
            Name = "Sala 05",
            CampusId = 1,
            Campus = "Campus Agreste",
            Capacity = 40,
            ClassesCount = 1,
            WeeklyHours = 3M,
            PeakStudents = 32,
            Schedules =
            [
                new ClassroomScheduleOut
                {
                    ClassId = 1,
                    Discipline = "Banco de Dados",
                    Period = "2026.1",
                    Status = ClassStatus.Started,
                    Students = 32,
                    Day = Day.Monday,
                    StartAt = Hour.H07_00,
                    EndAt = Hour.H10_00,
                    Teachers = ["Chico Ferreira", "Ana Lima"],
                },
            ],
        }),
    ];
}

public class ClassroomScheduleOut
{
    public int ClassId { get; set; }
    public string Discipline { get; set; }
    public string Period { get; set; }

    /// <summary>
    /// Status da turma alocada
    /// </summary>
    public ClassStatus Status { get; set; }

    /// <summary>
    /// Quantidade de alunos matriculados na turma
    /// </summary>
    public int Students { get; set; }

    public List<string> Teachers { get; set; } = [];
    public Day Day { get; set; }
    public Hour StartAt { get; set; }
    public Hour EndAt { get; set; }
}
