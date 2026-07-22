namespace Estud.Back.Features.Students.GetStudentCourseDetails;

public class GetStudentCourseDetailsOut : IApiDto<GetStudentCourseDetailsOut>
{
    public int CourseOfferingId { get; set; }
    public string Course { get; set; }
    public string Curriculum { get; set; }
    public string Campus { get; set; }
    public string Period { get; set; }
    public CourseSession Session { get; set; }

    /// <summary>
    /// Grade curricular do curso, com o status de cada disciplina em relação ao aluno.
    /// </summary>
    public List<GetStudentCourseDetailsDisciplineOut> Disciplines { get; set; } = [];

    public static IEnumerable<(string, GetStudentCourseDetailsOut)> GetExamples() =>
    [
        ("Exemplo", new GetStudentCourseDetailsOut
        {
            CourseOfferingId = 1,
            Course = "Análise e Desenvolvimento de Sistemas",
            Curriculum = "Grade ADS 2024",
            Campus = "Campus Maceió",
            Period = "2026.1",
            Session = CourseSession.Evening,
            Disciplines =
            [
                new GetStudentCourseDetailsDisciplineOut
                {
                    Id = 1,
                    Name = "Algoritmos",
                    Period = 1,
                    Credits = 4,
                    Workload = 60,
                    Status = StudentDisciplineStatus.Aprovada,
                },
                new GetStudentCourseDetailsDisciplineOut
                {
                    Id = 2,
                    Name = "Banco de Dados",
                    Period = 2,
                    Credits = 4,
                    Workload = 60,
                    Status = StudentDisciplineStatus.Cursando,
                },
                new GetStudentCourseDetailsDisciplineOut
                {
                    Id = 3,
                    Name = "Engenharia de Software",
                    Period = 3,
                    Credits = 4,
                    Workload = 60,
                    Status = StudentDisciplineStatus.NaoCursada,
                },
            ],
        }),
    ];
}

public class GetStudentCourseDetailsDisciplineOut
{
    public int Id { get; set; }
    public string Name { get; set; }

    /// <summary>
    /// Período (semestre) da disciplina na grade curricular.
    /// </summary>
    public byte Period { get; set; }
    public byte Credits { get; set; }
    public ushort Workload { get; set; }

    /// <summary>
    /// Status da disciplina em relação ao aluno.
    /// </summary>
    public StudentDisciplineStatus Status { get; set; }
}
