namespace Syki.Back.Features.Academic.CreateClass;

public class Schedule
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public Day Day { get; set; }
    public Hour StartAt { get; set; }
    public Hour EndAt { get; set; }

    private Schedule() {}

    public Schedule(
        Day day,
        Hour startAt,
        Hour endAt
    ) {
        Id = Guid.NewGuid();
        Day = day;
        StartAt = startAt;
        EndAt = endAt;
    }

    public static OneOf<Schedule, SykiError> New(
        Day day,
        Hour startAt,
        Hour endAt
    ) {
        if (startAt == endAt || endAt < startAt)
            return new InvalidSchedule();

        return new Schedule(day, startAt, endAt);
    }

    public bool Conflict(Schedule other)
    {
        if (Day != other.Day)
            return false;

        if (StartAt == other.StartAt || EndAt == other.EndAt)
            return true;

        if (StartAt < other.StartAt && other.StartAt < EndAt)
            return true;

        if (StartAt < other.EndAt && other.EndAt < EndAt)
            return true;

        if (other.StartAt < StartAt && StartAt < other.EndAt)
            return true;

        if (other.StartAt < EndAt && EndAt < other.EndAt)
            return true;

        return false;
    }

    public override string ToString()
    {
        return $"{Day.GetDescription()} {StartAt.GetDescription()}-{EndAt.GetDescription()}";
    }

    public ScheduleOut ToOut()
    {
        return new ScheduleOut
        {
            Day = Day,
            StartAt = StartAt,
            EndAt = EndAt,
        };
    }
}
