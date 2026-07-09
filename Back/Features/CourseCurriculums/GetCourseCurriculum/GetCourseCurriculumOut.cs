namespace Estud.Back.Features.CourseCurriculums.GetCourseCurriculum;

public class GetCourseCurriculumOut : IApiDto<GetCourseCurriculumOut>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CourseId { get; set; }
    public string Course { get; set; }
    public List<GetCourseCurriculumDisciplineOut> Disciplines { get; set; } = [];

    public static IEnumerable<(string, GetCourseCurriculumOut)> GetExamples() =>
    [
        ("Exemplo", new()
        {
            Id = 1,
            Name = "Grade ADS 2024",
            Course = "Análise e Desenvolvimento de Sistemas",
            Disciplines = [new() { Id = 1, Name = "Cálculo I", Period = 1, Credits = 4, Workload = 60 }],
        }),
    ];
}

public class GetCourseCurriculumDisciplineOut
{
    public int Id { get; set; }
    public string Name { get; set; }
    public byte Period { get; set; }
    public byte Credits { get; set; }
    public ushort Workload { get; set; }
}
