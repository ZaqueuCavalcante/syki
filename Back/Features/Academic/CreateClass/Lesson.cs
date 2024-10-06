using Syki.Back.Features.Teacher.CreateLessonAttendance;

namespace Syki.Back.Features.Academic.CreateClass;

/// <summary>
/// Representa uma Aula dentro de uma Turma.
/// </summary>
public class Lesson
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public DateOnly Date { get; set; }
    public Hour StartAt { get; set; }
    public Hour EndAt { get; set; }
    public LessonStatus Status { get; set; }
    public List<LessonAttendance> Attendances { get; set; }

    public Lesson(
        Guid classId,
        DateOnly date,
        Hour startAt,
        Hour endAt
    ) {
        Id = Guid.NewGuid();
        ClassId = classId;
        Date = date;
        StartAt = startAt;
        EndAt = endAt;
        Status = LessonStatus.Pending;
    }

    public void Finish()
    {
        Status = LessonStatus.Finalized;
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

    public LessonOut ToOut(int number)
    {
        return new()
        {
            Id = Id,
            Number = number,
            Date = Date,
            Status = Status,
            Schedule = GetSchedule(),
            Frequency = GetFrequency(),
        };
    }
}
