namespace Estud.Back.Features.CourseCurriculums.UpdateCourseCurriculum;

public class UpdateCourseCurriculumIn : IApiDto<UpdateCourseCurriculumIn>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<UpdateCourseCurriculumDisciplineIn> Disciplines { get; set; } = [];

    public static IEnumerable<(string, UpdateCourseCurriculumIn)> GetExamples() =>
    [
        (
            "Exemplo",
            new()
            {
                Id = 1,
                Name = "Grade ADS 2024",
                Disciplines = [new() { Id = 1, Period = 1, Credits = 4, Workload = 60 }],
            }
        ),
    ];
}

public class UpdateCourseCurriculumDisciplineIn
{
    public int Id { get; set; }
    public byte Period { get; set; }
    public byte Credits { get; set; }
    public ushort Workload { get; set; }
}
