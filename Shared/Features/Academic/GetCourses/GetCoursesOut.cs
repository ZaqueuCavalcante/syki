namespace Syki.Shared;

public class GetCoursesOut : IApiDto<GetCoursesOut>
{
    public int Total { get; set; }
    public List<GetCoursesItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetCoursesOut)> GetExamples() =>
    [
        ("Courses",
        new GetCoursesOut()
        {
            Total = 3,
            Items =
            [
                new GetCoursesItemOut
                {
                    Id = Guid.NewGuid(),
                    Name = "ADS",
                    Type = CourseType.Tecnologo,
                },
                new GetCoursesItemOut
                {
                    Id = Guid.NewGuid(),
                    Name = "Medicina",
                    Type = CourseType.Bacharelado,
                },
                new GetCoursesItemOut
                {
                    Id = Guid.NewGuid(),
                    Name = "Direito",
                    Type = CourseType.Bacharelado,
                },
            ],
        }),
    ];
}

public class GetCoursesItemOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CourseType Type { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((GetCoursesItemOut)obj).Id;
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
