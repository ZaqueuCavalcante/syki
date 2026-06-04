namespace Syki.Back.Commands.Domain.Classes;

public class Schedule
{
    public int Id { get; set; }
    public int? ClassId { get; set; }
    public int? ClassroomId { get; set; }
    public int? TeacherId { get; set; }
    public Day Day { get; set; }
    public Hour Start { get; set; }
    public Hour End { get; set; }

    private Schedule() {}

    public Schedule(
        Day day,
        Hour startAt,
        Hour endAt
    ) {
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
}
