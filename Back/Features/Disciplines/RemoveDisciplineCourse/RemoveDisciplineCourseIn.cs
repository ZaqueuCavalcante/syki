namespace Estud.Back.Features.Disciplines.RemoveDisciplineCourse;

public class RemoveDisciplineCourseIn : IApiDto<RemoveDisciplineCourseIn>
{
    public int DisciplineId { get; set; }
    public int CourseId { get; set; }

    public static IEnumerable<(string, RemoveDisciplineCourseIn)> GetExamples() =>
    [
        ("Exemplo", new() { DisciplineId = 1, CourseId = 1 }),
    ];
}
