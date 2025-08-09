namespace Syki.Shared;

public class DisciplineOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public byte Period { get; set; }
    public byte Credits { get; set; }
    public ushort Workload { get; set; }
    public int Teachers { get; set; }
    public List<Guid> PreRequisites { get; set; }
    public StudentDisciplineStatus StudentDisciplineStatus { get; set; }
    public List<Guid> Courses { get; set; } = [];

    public static IEnumerable<(string, DisciplineOut)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((DisciplineOut)obj).Id;
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
