namespace Syki.Back.Shared;

public class CreateCourseCurriculumIn : IApiDto<CreateCourseCurriculumIn>
{
    public string Name { get; set; }
    public int CourseId { get; set; }
    public List<CreateCourseCurriculumDisciplineIn> Disciplines { get; set; } = [];

    public static IEnumerable<(string, CreateCourseCurriculumIn)> GetExamples() =>
    [
        (
            "Grade ADS",
            new()
            {
                Name = "Grade ADS",
                CourseId = 1,
                Disciplines = [new(1, 1, 55, 70)]
            }
        ),
    ];
}
