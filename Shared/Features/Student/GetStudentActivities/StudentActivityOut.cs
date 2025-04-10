namespace Syki.Shared;

public class StudentActivityOut
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public string ClassName { get; set; }
    public ClassNoteType Note { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ClassActivityType Type { get; set; }
    public int Weight { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateOnly DueDate { get; set; }
    public Hour DueHour { get; set; }

    public string GetDueDate()
    {
        return $"{DueDate} {DueHour.GetDescription()}";
    }
}
