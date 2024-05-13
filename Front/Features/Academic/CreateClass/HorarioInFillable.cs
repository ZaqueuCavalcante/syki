namespace Syki.Front.Features.Academic.CreateClass;

public class ScheduleInFillable
{
    public Guid Id { get; set; }
    public Day? Day { get; set; }
    public Hour? Start { get; set; }
    public Hour? End { get; set; }

    public ScheduleInFillable()
    {
        Id = Guid.NewGuid();
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((ScheduleInFillable)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.ToHashCode();
    }
}
