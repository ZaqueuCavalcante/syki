namespace Syki.Shared;

public class GetAcademicClassOut
{
    public Guid Id { get; set; }
    public string Discipline { get; set; }
    public string Code { get; set; }
    public string Teacher { get; set; }
    public string Period { get; set; }
    public int Vacancies { get; set; }
    public ClassStatus Status { get; set; }
    public List<ScheduleOut> Schedules { get; set; }
    public List<LessonOut> Lessons { get; set; } = [];
    public string SchedulesInline { get; set; }
    public string FillRatio { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((GetAcademicClassOut)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.ToHashCode();
    }

    public static implicit operator GetAcademicClassOut(OneOf<GetAcademicClassOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
