namespace Syki.Shared;

public class StudentClassActivityOut
{
    public Guid Id { get; set; }
    public ClassNoteType Note { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ClassActivityType Type { get; set; }
    public ClassActivityStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateOnly DueDate { get; set; }
    public Hour DueHour { get; set; }
    public int Weight { get; set; }
    public decimal Value { get; set; }
    public decimal PonderedValue { get; set; }
    public ClassActivityWorkStatus WorkStatus { get; set; }

    public string GetDueDate()
    {
        return $"{DueDate} {DueHour.GetDescription()}";
    }
}
