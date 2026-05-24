namespace Syki.Back.Features.Disciplines.AddDisciplineCourses;

public class AddDisciplineCoursesIn : IApiDto<AddDisciplineCoursesIn>
{
    public int DisciplineId { get; set; }
    public List<int> Courses { get; set; } = [];

    public static IEnumerable<(string, AddDisciplineCoursesIn)> GetExamples() =>
    [
        ("Exemplo", new() { DisciplineId = 1, Courses = [1, 2] }),
    ];
}
