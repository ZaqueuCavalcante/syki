namespace Syki.Shared;

public class TeacherClassActivityOut
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Weight { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateOnly? DueDate { get; set; }
}
