namespace Estud.Back.Features.CourseCurriculums.CreateCourseCurriculum;

public class CreateCourseCurriculumOut : IApiDto<CreateCourseCurriculumOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, CreateCourseCurriculumOut)> GetExamples() =>
    [
        ("Exemplo", new() { Id = 1 }),
    ];
}
