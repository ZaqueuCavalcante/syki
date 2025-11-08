namespace Syki.Shared;

public class GetCoursesWithCurriculumsOut : IApiDto<GetCoursesWithCurriculumsOut>
{
    public int Total { get; set; }
    public List<GetCoursesWithCurriculumsItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetCoursesWithCurriculumsOut)> GetExamples() =>
    [
        ("Courses",
        new GetCoursesWithCurriculumsOut()
        {
            Total = 3,
            Items =
            [
                new GetCoursesWithCurriculumsItemOut
                {
                    Id = Guid.NewGuid(),
                    Name = "ADS",
                    Type = CourseType.Tecnologo,
                },
                new GetCoursesWithCurriculumsItemOut
                {
                    Id = Guid.NewGuid(),
                    Name = "Medicina",
                    Type = CourseType.Bacharelado,
                },
                new GetCoursesWithCurriculumsItemOut
                {
                    Id = Guid.NewGuid(),
                    Name = "Direito",
                    Type = CourseType.Bacharelado,
                },
            ],
        }),
    ];
}

public class GetCoursesWithCurriculumsItemOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CourseType Type { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((GetCoursesWithCurriculumsItemOut)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.ToHashCode();
    }

    public override string ToString()
    {
        return Name;
    }
}
