namespace Syki.Shared;

public class ScheduleIn
{
    public Guid Id { get; set; }
    public Day Day { get; set; }
    public Hour Start { get; set; }
    public Hour End { get; set; }

    public ScheduleIn()
    {
        Id = Guid.CreateVersion7();
    }

    public ScheduleIn(
        Day dia,
        Hour start,
        Hour end
    ) {
        Id = Guid.CreateVersion7();
        Day = dia;
        Start = start;
        End = end;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        var other = (ScheduleIn) obj;

        if (other.Id == Id)
            return true;

        return other.Day == Day && other.Start == Start && other.End == End;
    }

    public override int GetHashCode()
    {
        return Id.ToHashCode();
    }
}
