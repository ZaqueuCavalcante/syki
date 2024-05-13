namespace Syki.Shared;

public class ScheduleOut
{
    public Day Day { get; set; }
    public Hour Start { get; set; }
    public Hour End { get; set; }

    public ScheduleOut() { }

    public ScheduleOut(
        Day day,
        Hour start,
        Hour end
    ) {
        Day = day;
        Start = start;
        End = end;
    }
}
