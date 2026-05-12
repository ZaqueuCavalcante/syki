namespace Syki.Back.Features.Academic.CreateClass;

public class Schedule
{
    public Guid Id { get; set; }
    public Guid? ClassId { get; set; }
    public Guid? ClassroomId { get; set; }
    public Guid? TeacherId { get; set; }
    public Day Day { get; set; }
    public Hour Start { get; set; }
    public Hour End { get; set; }

    private Schedule() {}

    public Schedule(
        Day day,
        Hour startAt,
        Hour endAt
    ) {
        Id = Guid.CreateVersion7();
        Day = day;
        Start = startAt;
        End = endAt;
    }

    public static OneOf<Schedule, SykiError> New(
        Day day,
        Hour startAt,
        Hour endAt
    ) {
        if (!day.IsValid()) return new InvalidDay();
        if (!startAt.IsValid()) return new InvalidHour();
        if (!endAt.IsValid()) return new InvalidHour();

        if (startAt == endAt || endAt < startAt)
            return new InvalidSchedule();

        return new Schedule(day, startAt, endAt);
    }

    public int GetDiff()
    {
        return Start.DiffInMinutes(End);
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
        return new()
        {
            Day = Day,
            StartAt = Start,
            EndAt = End,
        };
    }
}
