namespace Syki.Back.Commands.Domain.Classes;

/// <summary>
/// Aula
/// </summary>
public class ClassLesson
{
    public int Id { get; set; }
    public int ClassId { get; set; }
    public int Number { get; set; }
    public DateOnly Date { get; set; }
    public Hour StartAt { get; set; }
    public Hour EndAt { get; set; }
    public ClassLessonStatus Status { get; set; }
    public List<ClassLessonAttendance> Attendances { get; set; }

    public Class Class { get; set; }

    private ClassLesson() {}

    public ClassLesson(
        Class @class,
        int number,
        DateOnly date,
        Hour startAt,
        Hour endAt
    ) {
        Class = @class;
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
}
