namespace Syki.Shared;

public class ClassOut : IApiDto<ClassOut>
{
    public Guid Id { get; set; }
    public string Discipline { get; set; }
    public string Teacher { get; set; }
    public string Period { get; set; }
    public int Vacancies { get; set; }
    public decimal Frequency { get; set; }
    public ClassStatus Status { get; set; }
    public List<ScheduleOut> Schedules { get; set; }
    public List<ClassLessonOut> Lessons { get; set; } = [];

    public string FillRatio { get; set; }
    public bool IsSelected { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((ClassOut)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.ToHashCode();
    }

    public static IEnumerable<(string, ClassOut)> GetExamples() =>
    [
        ("Banco de Dados",
        new ClassOut
        {
            Id = Guid.CreateVersion7(),
            Discipline = "Banco de Dados",
        }),
        ("Programação Orientada a Objetos",
        new ClassOut
        {
            Id = Guid.CreateVersion7(),
            Discipline = "Programação Orientada a Objetos",
        }),
    ];

    public static implicit operator ClassOut(OneOf<ClassOut, ErrorOut> value)
    {
        return value.Success;
    }
}
