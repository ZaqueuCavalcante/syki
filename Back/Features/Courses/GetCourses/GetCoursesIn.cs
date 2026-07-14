namespace Estud.Back.Features.Courses.GetCourses;

public class GetCoursesIn : IApiDto<GetCoursesIn>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public static IEnumerable<(string, GetCoursesIn)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}
