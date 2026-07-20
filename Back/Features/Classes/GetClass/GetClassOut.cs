namespace Estud.Back.Features.Classes.GetClass;

public class GetClassOut : IApiDto<GetClassOut>
{
    public int Id { get; set; }
    public int DisciplineId { get; set; }
    public string Discipline { get; set; }
    public string Period { get; set; }
    public int? CampusId { get; set; }
    public int Vacancies { get; set; }
    public int Workload { get; set; }
    public ClassStatus Status { get; set; }
    public List<GetClassTeacherOut> Teachers { get; set; } = [];
    public List<ScheduleOut> Schedules { get; set; } = [];
    public List<GetClassStudentOut> Students { get; set; } = [];

    public static IEnumerable<(string, GetClassOut)> GetExamples() =>
    [
        ("Exemplo", new GetClassOut
        {
            Id = 1,
            DisciplineId = 3,
            Discipline = "Banco de Dados",
            Period = "2026.1",
            CampusId = 1,
            Vacancies = 40,
            Workload = 60,
            Status = ClassStatus.OnEnrollment,
            Teachers =
            [
                new GetClassTeacherOut { Id = 14, Name = "Ana Lima" },
                new GetClassTeacherOut { Id = 32, Name = "Chico Ferreira" },
            ],
            Schedules = [new ScheduleOut(Day.Monday, Hour.H07_00, Hour.H10_00) { TeacherId = 14, Teacher = "Ana Lima", ClassroomId = 5, Classroom = "Sala 05" }],
            Students =
            [
                new GetClassStudentOut { Id = 1, Name = "Maria Souza", Status = StudentClassStatus.Matriculado, AverageGrade = 8.5M, AverageAttendance = 92.0M },
            ],
        }),
    ];
}

public class GetClassTeacherOut
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class GetClassStudentOut
{
    public int Id { get; set; }
    public string Name { get; set; }
    public StudentClassStatus Status { get; set; }

    /// <summary>
    /// Nota média do aluno na turma (de 0 a 10)
    /// </summary>
    public decimal AverageGrade { get; set; }

    /// <summary>
    /// Frequência média do aluno na turma (de 0% a 100%)
    /// </summary>
    public decimal AverageAttendance { get; set; }
}
