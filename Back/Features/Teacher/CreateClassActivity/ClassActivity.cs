using Syki.Back.Features.Student.CreateClassActivityWork;

namespace Syki.Back.Features.Teacher.CreateClassActivity;

/// <summary>
/// Atividade vinculada à uma Turma
/// </summary>
public class ClassActivity : Entity
{
    public Guid Id { get; }
    public Guid ClassId { get; set; }
    public ClassNoteType Note { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public ClassActivityType Type { get; set; }
    public ClassActivityStatus Status { get; set; }

    /// <summary>
    /// Peso no intervalo: 0 ≤ Weight ≤ 100
    /// </summary>
    public int Weight { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateOnly DueDate { get; set; }
    public Hour DueHour { get; set; }

    public List<ClassActivityWork> Works { get; set; }

    private ClassActivity() {}

    private ClassActivity(
        Guid classId,
        ClassNoteType note,
        string title,
        string description,
        ClassActivityType type,
        int weight,
        DateOnly dueDate,
        Hour dueHour,
        List<Guid> students
    ) {
        Id = Guid.NewGuid();
        ClassId = classId;
        Note = note;
        Title = title;
        Description = description;
        Type = type;
        Weight = weight;
        CreatedAt = DateTime.Now;
        DueDate = dueDate;
        DueHour = dueHour;

        Works = students.ConvertAll(x => new ClassActivityWork(Id, x));

        AddDomainEvent(new ClassActivityCreatedDomainEvent(Id));
    }

    public static OneOf<ClassActivity, SykiError> New(
        Guid classId,
        ClassNoteType note,
        string title,
        string description,
        ClassActivityType type,
        int weight,
        DateOnly dueDate,
        Hour dueHour,
        List<Guid> students
    ) {
        if (weight < 0 || weight > 100) return new InvalidClassActivityWeight();

        return new ClassActivity(classId, note, title, description, type, weight, dueDate, dueHour, students);
    }

    public TeacherClassActivityOut ToOut()
    {
        return new()
        {
            Id = Id,
            ClassId = ClassId,
            Note = Note,
            Title = Title,
            Description = Description,
            Type = Type,
            Weight = Weight,
            CreatedAt = CreatedAt,
            DueDate = DueDate,
            DueHour = DueHour,
            Works = Works != null ? Works.Select(w => w.ToOut()).ToList() : [],
        };
    }

    public StudentActivityOut ToStudentActivityOut(string className)
    {
        return new()
        {
            Id = Id,
            ClassId = ClassId,
            ClassName = className,
            Note = Note,
            Title = Title,
            Description = Description,
            Type = Type,
            Weight = Weight,
            CreatedAt = CreatedAt,
            DueDate = DueDate,
            DueHour = DueHour,
        };
    }

    public CreateClassActivityOut ToCreateOut()
    {
        return new()
        {
            Id = Id,
        };
    }
}
