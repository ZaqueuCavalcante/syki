namespace Syki.Back.Features.Teacher.CreateClassActivity;

/// <summary>
/// Atividade vinculada à uma Turma
/// </summary>
public class ClassActivity : Entity
{
    public Guid Id { get; }
    public Guid ClassId { get; set; }
    public Guid LessonId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    /// <summary>
    /// Nota no intervalo: 0.00 ≤ Value ≤ 10.00
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// Peso no intervalo: 0 ≤ Weight ≤ 100
    /// </summary>
    public int Weight { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }

    public ClassActivity(
        Guid classId,
        Guid lessonId,
        string title,
        string description,
        DateTime? dueDate
    ) {
        Id = Guid.NewGuid();
        ClassId = classId;
        LessonId = lessonId;
        Title = title;
        Description = description;
        CreatedAt = DateTime.Now;
        DueDate = dueDate;

        AddDomainEvent(new ClassActivityCreatedDomainEvent(ClassId));
    }
}
