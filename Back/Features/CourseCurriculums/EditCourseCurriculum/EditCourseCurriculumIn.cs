namespace Syki.Back.Features.CourseCurriculums.EditCourseCurriculum;

public class EditCourseCurriculumIn : IApiDto<EditCourseCurriculumIn>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<EditCourseCurriculumDisciplineIn> Disciplines { get; set; } = [];

    public static IEnumerable<(string, EditCourseCurriculumIn)> GetExamples() =>
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

public class EditCourseCurriculumDisciplineIn
{
    public int Id { get; set; }
    public byte Period { get; set; }
    public byte Credits { get; set; }
    public ushort Workload { get; set; }
}
