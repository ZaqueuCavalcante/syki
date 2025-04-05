namespace Syki.Shared;

public class TeacherClassOut
{
    public Guid Id { get; set; }
    public string Discipline { get; set; }
    public string Code { get; set; }
    public string Period { get; set; }
    public ClassStatus Status { get; set; }
    public List<LessonOut> Lessons { get; set; } = [];
    public List<TeacherClassStudentOut> Students { get; set; } = [];

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((TeacherClassesOut)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.ToHashCode();
    }
}
