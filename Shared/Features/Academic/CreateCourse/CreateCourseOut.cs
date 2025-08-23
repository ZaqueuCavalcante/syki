namespace Syki.Shared;

public class CreateCourseOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CourseType Type { get; set; }
    public List<CreateCourseDisciplineOut> Disciplines { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((CreateCourseOut)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.ToHashCode();
    }

    public override string ToString()
    {
        return Name;
    }

    public static IEnumerable<(string, CreateCourseOut)> GetExamples() =>
    [
        ("Direito",
        new CreateCourseOut
        {
            Id = Guid.CreateVersion7(),
            Name = "Direito",
            Type = CourseType.Bacharelado,
            Disciplines = [new() { Id = Guid.NewGuid(), Name = "Direito Civil" }, new() { Id = Guid.NewGuid(), Name = "Direito Penal" }],
        }),
        ("ADS",
        new CreateCourseOut
        {
            Id = Guid.CreateVersion7(),
            Name = "Análise e Desenvolvimento de Sistemas",
            Type = CourseType.Tecnologo,
            Disciplines = [new() { Id = Guid.NewGuid(), Name = "Programação Orientada a Objetos" }, new() { Id = Guid.NewGuid(), Name = "Banco de Dados" }],
        }),
    ];

    public static implicit operator CreateCourseOut(OneOf<CreateCourseOut, ErrorOut> value)
    {
        return value.Success;
    }
}

public class CreateCourseDisciplineOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
}
