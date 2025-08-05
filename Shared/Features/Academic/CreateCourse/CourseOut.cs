namespace Syki.Shared;

public class CourseOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CourseType Type { get; set; }
    public List<DisciplineOut> Disciplines { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((CourseOut)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.ToHashCode();
    }

    public override string ToString()
    {
        return Name;
    }

    public static IEnumerable<(string, CourseOut)> GetExamples() =>
    [
        ("Direito",
        new CourseOut
        {
            Id = Guid.CreateVersion7(),
            Name = "Direito",
            Type = CourseType.Bacharelado,
            Disciplines = [],
        }),
        ("ADS",
        new CourseOut
        {
            Id = Guid.CreateVersion7(),
            Name = "Análise e Desenvolvimento de Sistemas",
            Type = CourseType.Tecnologo,
            Disciplines = [],
        }),
    ];

    public static implicit operator CourseOut(OneOf<CourseOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
