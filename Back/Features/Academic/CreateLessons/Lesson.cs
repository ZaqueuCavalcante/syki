namespace Syki.Back.Features.Academic.CreateLessons;

/// <summary>
/// Representa uma Aula dentro de uma Turma.
/// </summary>
public class Lesson
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public int Number { get; set; }
    public DateOnly Date { get; set; }

    public Lesson(
        Guid classId,
        int number,
        DateOnly date
    ) {
        Id = Guid.NewGuid();
        ClassId = classId;
        Number = number;
        Date = date;
    }

    public LessonOut ToOut()
    {
        return new()
        {
            Id = Id,
            Number = Number,
            Date = Date,
        };
    }
}
