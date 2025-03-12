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
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }

    public ClassActivity(
        Guid classId,
        string title,
        string description,
        DateTime? dueDate
    ) {
        Id = Guid.NewGuid();
        ClassId = classId;
        Title = title;
        Description = description;
        CreatedAt = DateTime.Now;
        DueDate = dueDate;

        AddDomainEvent(new ClassActivityCreatedDomainEvent(ClassId));
    }
}
