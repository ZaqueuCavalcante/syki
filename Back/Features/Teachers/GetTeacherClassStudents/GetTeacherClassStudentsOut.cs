namespace Estud.Back.Features.Teachers.GetTeacherClassStudents;

public class GetTeacherClassStudentsOut : IApiDto<GetTeacherClassStudentsOut>
{
    public List<GetTeacherClassStudentsItemOut> Students { get; set; } = [];

    public static IEnumerable<(string, GetTeacherClassStudentsOut)> GetExamples() =>
    [
        ("Exemplo", new GetTeacherClassStudentsOut
        {
            Students =
            [
                new()
                {
                    Id = 1,
                    Name = "Zaqueu do Vale",
                    Status = StudentClassStatus.Matriculado,
                    AverageGrade = 8.5M,
                    AverageAttendance = 92.0M,
                },
            ],
        }),
    ];
}

public class GetTeacherClassStudentsItemOut
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
