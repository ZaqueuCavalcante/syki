namespace Syki.Shared;

public class ClassLessonOut
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public DateOnly Date { get; set; }
    public string Schedule { get; set; }
    public ClassLessonStatus Status { get; set; }
    public decimal Frequency { get; set; }

    public static IEnumerable<(string, ClassLessonOut)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((ClassLessonOut)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.ToHashCode();
    }

    public override string ToString()
    {
        return $"Aula {Number.ToTwo()}";
    }

    public static implicit operator ClassLessonOut(OneOf<ClassLessonOut, ErrorOut> value)
    {
        return value.Success;
    }
}
