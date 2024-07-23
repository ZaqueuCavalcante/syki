namespace Syki.Shared;

public class TeacherClassesOut
{
    public Guid Id { get; set; }
    public string Discipline { get; set; }
    public string Code { get; set; }
    public string Period { get; set; }
    public List<ScheduleOut> Schedules { get; set; }
    public string SchedulesInline { get; set; }

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
