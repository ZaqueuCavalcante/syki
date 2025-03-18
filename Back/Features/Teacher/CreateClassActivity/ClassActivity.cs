namespace Syki.Back.Features.Teacher.CreateClassActivity;

/// <summary>
/// Atividade vinculada à uma Turma
/// </summary>
public class ClassActivity : Entity
{
    public Guid Id { get; }
    public Guid ClassId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ClassActivityType Type { get; set; }
    public ClassActivityStatus Status { get; set; }

    /// <summary>
    /// Peso no intervalo: 0 ≤ Weight ≤ 100
    /// </summary>
    public int Weight { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateOnly? DueDate { get; set; }
    public Hour? DueHour { get; set; }

    public ClassActivity(
        Guid classId,
        string title,
        string description,
        DateOnly? dueDate,
        Hour? dueHour
    ) {
        Id = Guid.NewGuid();
        ClassId = classId;
        Title = title;
        Description = description;
        CreatedAt = DateTime.Now;
        DueDate = dueDate;
        DueHour = dueHour;

        AddDomainEvent(new ClassActivityCreatedDomainEvent(Id));
    }

    public TeacherClassActivityOut ToOut()
    {
        return new()
        {
            Id = Id,
            ClassId = ClassId,
            Title = Title,
            Description = Description,
            Weight = Weight,
            CreatedAt = CreatedAt,
            DueDate = DueDate,
        };
    }
}
