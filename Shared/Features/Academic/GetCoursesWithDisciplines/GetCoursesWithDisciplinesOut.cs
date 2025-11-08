namespace Syki.Shared;

public class GetCoursesWithDisciplinesOut : IApiDto<GetCoursesWithDisciplinesOut>
{
    public int Total { get; set; }
    public List<GetCoursesWithDisciplinesItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetCoursesWithDisciplinesOut)> GetExamples() =>
    [
        ("Courses",
        new GetCoursesWithDisciplinesOut()
        {
            Total = 3,
            Items =
            [
                new GetCoursesWithDisciplinesItemOut
                {
                    Id = Guid.NewGuid(),
                    Name = "ADS",
                    Type = CourseType.Tecnologo,
                },
                new GetCoursesWithDisciplinesItemOut
                {
                    Id = Guid.NewGuid(),
                    Name = "Medicina",
                    Type = CourseType.Bacharelado,
                },
                new GetCoursesWithDisciplinesItemOut
                {
                    Id = Guid.NewGuid(),
                    Name = "Direito",
                    Type = CourseType.Bacharelado,
                },
            ],
        }),
    ];
}

public class GetCoursesWithDisciplinesItemOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CourseType Type { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((GetCoursesWithDisciplinesItemOut)obj).Id;
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
