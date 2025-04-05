using Syki.Back.Features.Teacher.CreateLessonAttendance;

namespace Syki.Back.Features.Academic.CreateClass;

/// <summary>
/// Aula
/// </summary>
public class ClassLesson
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public int Number { get; set; }
    public DateOnly Date { get; set; }
    public Hour StartAt { get; set; }
    public Hour EndAt { get; set; }
    public ClassLessonStatus Status { get; set; }
    public List<ClassLessonAttendance> Attendances { get; set; }

    private ClassLesson() {}

    public ClassLesson(
        Guid classId,
        int number,
        DateOnly date,
        Hour startAt,
        Hour endAt
    ) {
        Id = Guid.NewGuid();
        ClassId = classId;
        Number = number;
        Date = date;
        StartAt = startAt;
        EndAt = endAt;
        Status = ClassLessonStatus.Pending;
    }

    public void Finish()
    {
        Status = ClassLessonStatus.Finalized;
    }

    private string GetSchedule()
    {
        return $"{Date.DayOfWeek.ToString().ToEnum<Day>().GetDescription()} {StartAt.GetDescription()}-{EndAt.GetDescription()}";
    }

    private decimal GetFrequency()
    {
        var total = Attendances.Count;
        if (total == 0) return 0.00M;
        var presences = Attendances.Count(x => x.Present);
        return 100M*(1M * presences / (1M * total));
    }

    public ClassLessonOut ToOut()
    {
        return new()
        {
            Id = Id,
            Number = Number,
            Date = Date,
            Status = Status,
            Schedule = GetSchedule(),
            Frequency = GetFrequency(),
        };
    }
}
