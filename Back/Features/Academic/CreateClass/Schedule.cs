namespace Syki.Back.Features.Academic.CreateClass;

public class Schedule
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public Day Day { get; set; }
    public Hour Start { get; set; }
    public Hour End { get; set; }

    private Schedule() {}

    public Schedule(
        Day dia,
        Hour start,
        Hour end
    ) {
        Id = Guid.NewGuid();
        Day = dia;

        if (start == end || end < start)
            Throw.DE021.Now();

        Start = start;
        End = end;
    }

    public bool Conflict(Schedule other)
    {
        if (Day != other.Day)
            return false;

        if (Start == other.Start || End == other.End)
            return true;

        if (Start < other.Start && other.Start < End)
            return true;

        if (Start < other.End && other.End < End)
            return true;

        if (other.Start < Start && Start < other.End)
            return true;

        if (other.Start < End && End < other.End)
            return true;

        return false;
    }

    public override string ToString()
    {
        return $"{Day.GetDescription()} {Start.GetDescription()}-{End.GetDescription()}";
    }

    public ScheduleOut ToOut()
    {
        return new ScheduleOut
        {
            Day = Day,
            Start = Start,
            End = End,
        };
    }
}
