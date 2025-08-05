namespace Syki.Shared;

public class CreateCourseCurriculumIn
{
    public string Name { get; set; }
    public Guid CourseId { get; set; }
    public List<CreateCourseCurriculumDisciplineIn> Disciplines { get; set; } = [];

    public static IEnumerable<(string, CreateCourseCurriculumIn)> GetExamples() =>
    [
        (
            "Grade ADS",
            new()
            {
                Name = "Grade ADS",
                CourseId = Guid.CreateVersion7(),
                Disciplines = [new(Guid.CreateVersion7(), 1, 55, 70)]
            }
        ),
    ];
}
