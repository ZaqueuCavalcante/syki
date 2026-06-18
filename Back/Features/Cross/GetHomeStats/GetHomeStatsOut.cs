namespace Syki.Back.Features.Cross.GetHomeStats;

public class GetHomeStatsOut : IApiDto<GetHomeStatsOut>
{
    public int EnrolledStudents { get; set; }
    public int ActiveTeachers { get; set; }
    public int OfferedCourses { get; set; }
    public int RegisteredDisciplines { get; set; }

    public static IEnumerable<(string, GetHomeStatsOut)> GetExamples() =>
    [
        ("Exemplo", new GetHomeStatsOut
        {
            EnrolledStudents = 1240,
            ActiveTeachers = 87,
            OfferedCourses = 14,
            RegisteredDisciplines = 132,
        }),
    ];
}
