using Syki.Back.Features.Teacher.CreateLessonAttendance;

namespace Syki.Back.Features.Academic.CreateLessons;

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

    private string GetSchedule()
    {
        return $"{Date.DayOfWeek.ToString().ToEnum<Day>().GetDescription()} {StartAt.GetDescription()}-{EndAt.GetDescription()}";
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
        };
    }
}
