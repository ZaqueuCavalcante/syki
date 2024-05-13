namespace Syki.Front.Features.Academic.CreateCourseCurriculum;

public class GradeDisciplineFillable
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public byte? Period { get; set; }
    public byte? Credits { get; set; }
    public ushort? Workload { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((GradeDisciplineFillable)obj).Id;
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
