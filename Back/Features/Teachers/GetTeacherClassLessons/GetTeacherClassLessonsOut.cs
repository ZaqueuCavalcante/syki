namespace Estud.Back.Features.Teachers.GetTeacherClassLessons;

public class GetTeacherClassLessonsOut : IApiDto<GetTeacherClassLessonsOut>
{
    public List<GetTeacherClassLessonsItemOut> Lessons { get; set; } = [];

    public static IEnumerable<(string, GetTeacherClassLessonsOut)> GetExamples() =>
    [
        ("Exemplo", new GetTeacherClassLessonsOut
        {
            Lessons =
            [
                new()
                {
                    Id = 1,
                    Number = 1,
                    Date = new DateOnly(2026, 3, 2),
                    StartAt = Hour.H19_00,
                    EndAt = Hour.H22_00,
                    Status = ClassLessonStatus.Finalized,
                    PresentStudents = [1, 2],
                },
                new()
                {
                    Id = 2,
                    Number = 2,
                    Date = new DateOnly(2026, 3, 9),
                    StartAt = Hour.H19_00,
                    EndAt = Hour.H22_00,
                    Status = ClassLessonStatus.Pending,
                    PresentStudents = [],
                },
            ],
        }),
    ];
}

public class GetTeacherClassLessonsItemOut
{
    public int Id { get; set; }
    public int Number { get; set; }
    public DateOnly Date { get; set; }
    public Hour StartAt { get; set; }
    public Hour EndAt { get; set; }
    public ClassLessonStatus Status { get; set; }
    public List<int> PresentStudents { get; set; } = [];
}
