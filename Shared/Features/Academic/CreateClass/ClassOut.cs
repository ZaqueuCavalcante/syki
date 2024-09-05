namespace Syki.Shared;

public class ClassOut
{
    public Guid Id { get; set; }
    public string Discipline { get; set; }
    public string Teacher { get; set; }
    public string Period { get; set; }
    public int Vacancies { get; set; }
    public decimal Frequency { get; set; }
    public ClassStatus Status { get; set; }
    public List<ScheduleOut> Schedules { get; set; }

    public string FillRatio { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((ClassOut)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.ToHashCode();
    }

    public static implicit operator ClassOut(OneOf<ClassOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
