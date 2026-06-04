namespace Syki.Back.Features.Courses.RemoveCourseDiscipline;

public class RemoveCourseDisciplineIn : IApiDto<RemoveCourseDisciplineIn>
{
    public int CourseId { get; set; }
    public int DisciplineId { get; set; }

    public static IEnumerable<(string, RemoveCourseDisciplineIn)> GetExamples() =>
    [
        ("Exemplo", new() { CourseId = 1, DisciplineId = 1 }),
    ];
}
