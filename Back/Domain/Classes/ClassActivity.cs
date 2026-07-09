namespace Estud.Back.Domain.Classes;

/// <summary>
/// Atividade vinculada à uma Turma
/// </summary>
public class ClassActivity
{
    public int Id { get; set; }
    public int ClassId { get; set; }
    public ClassNoteType Note { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public ClassActivityType ActivityType { get; set; }
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
        int classId,
        ClassNoteType note,
        string title,
        string description,
        ClassActivityType type,
        int weight,
        DateOnly dueDate,
        Hour dueHour,
        List<int> students
    ) {
        ClassId = classId;
        Note = note;
        Title = title;
        Description = description;
        ActivityType = type;
        Weight = weight;
        CreatedAt = DateTime.UtcNow;
        DueDate = dueDate;
        DueHour = dueHour;

        Works = students.ConvertAll(x => new ClassActivityWork(Id, x));
    }

    public static OneOf<ClassActivity, EstudError> New(
        int classId,
        ClassNoteType note,
        string title,
        string description,
        ClassActivityType type,
        int weight,
        DateOnly dueDate,
        Hour dueHour,
        List<int> students
    ) {
        if (weight < 0 || weight > 100) return new InvalidClassActivityWeight();

        return new ClassActivity(classId, note, title, description, type, weight, dueDate, dueHour, students);
    }
}
