namespace Syki.Back.Features.Disciplines.AddDisciplineCourses;

public class AddDisciplineCoursesOut : IApiDto<AddDisciplineCoursesOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, AddDisciplineCoursesOut)> GetExamples() =>
    [
        ("Exemplo", new() { Id = 1 }),
    ];
}
