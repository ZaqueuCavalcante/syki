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

    public static implicit operator CourseOut(OneOf<CourseOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
