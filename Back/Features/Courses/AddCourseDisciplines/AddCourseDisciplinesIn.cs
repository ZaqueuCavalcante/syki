namespace Syki.Back.Features.Courses.AddCourseDisciplines;

public class AddCourseDisciplinesIn : IApiDto<AddCourseDisciplinesIn>
{
    public int CourseId { get; set; }
    public List<int> Disciplines { get; set; } = [];

    public static IEnumerable<(string, AddCourseDisciplinesIn)> GetExamples() =>
    [
        ("Exemplo", new() { CourseId = 1, Disciplines = [1, 2] }),
    ];
}
