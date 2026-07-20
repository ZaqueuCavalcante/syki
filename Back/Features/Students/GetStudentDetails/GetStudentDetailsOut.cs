namespace Estud.Back.Features.Students.GetStudentDetails;

public class GetStudentDetailsOut : IApiDto<GetStudentDetailsOut>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string EnrollmentCode { get; set; }
    public StudentStatus Status { get; set; }

    /// <summary>
    /// Coeficiente de rendimento do aluno (de 0 a 10)
    /// </summary>
    public decimal YieldCoefficient { get; set; }

    /// <summary>
    /// Nota média do aluno nas turmas em andamento (de 0 a 10)
    /// </summary>
    public decimal AverageGrade { get; set; }

    /// <summary>
    /// Frequência média do aluno nas turmas em andamento (de 0% a 100%)
    /// </summary>
    public decimal AverageAttendance { get; set; }

    /// <summary>
    /// Oferta de curso atual do aluno. Nulo quando o aluno ainda não foi matriculado em nenhuma.
    /// </summary>
    public GetStudentDetailsCourseOut? Course { get; set; }

    public List<GetStudentDetailsClassOut> Classes { get; set; } = [];

    public static IEnumerable<(string, GetStudentDetailsOut)> GetExamples() =>
    [
        ("Exemplo", new GetStudentDetailsOut
        {
            Id = 1,
            Name = "Maria Souza",
            Email = "maria@ufal.edu.br",
            EnrollmentCode = "20251A2B3C4D",
            Status = StudentStatus.Enrolled,
            YieldCoefficient = 8.2M,
            AverageGrade = 8.5M,
            AverageAttendance = 92.0M,
            Course = new GetStudentDetailsCourseOut
            {
                CourseOfferingId = 1,
                Course = "Análise e Desenvolvimento de Sistemas",
                Campus = "Campus Maceió",
                Period = "2026.1",
                Session = CourseSession.Evening,
                EnrolledAt = new DateTime(2026, 2, 10, 0, 0, 0, DateTimeKind.Utc),
            },
            Classes =
            [
                new GetStudentDetailsClassOut
                {
                    Id = 7,
                    Discipline = "Banco de Dados",
                    Period = "2026.1",
                    Workload = 60,
                    Status = ClassStatus.Started,
                    MyStatus = StudentClassStatus.Matriculado,
                    AverageGrade = 8.5M,
                    AverageAttendance = 92.0M,
                },
            ],
        }),
    ];
}

public class GetStudentDetailsCourseOut
{
    public int CourseOfferingId { get; set; }
    public string Course { get; set; }
    public string Campus { get; set; }
    public string Period { get; set; }
    public CourseSession Session { get; set; }
    public DateTime EnrolledAt { get; set; }
}

public class GetStudentDetailsClassOut
{
    public int Id { get; set; }
    public string Discipline { get; set; }
    public string Period { get; set; }
    public int Workload { get; set; }
    public ClassStatus Status { get; set; }

    /// <summary>
    /// Status do aluno na turma
    /// </summary>
    public StudentClassStatus MyStatus { get; set; }

    /// <summary>
    /// Nota média do aluno na turma (de 0 a 10)
    /// </summary>
    public decimal AverageGrade { get; set; }

    /// <summary>
    /// Frequência média do aluno na turma (de 0% a 100%)
    /// </summary>
    public decimal AverageAttendance { get; set; }
}
