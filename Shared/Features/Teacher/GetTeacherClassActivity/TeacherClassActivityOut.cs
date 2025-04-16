namespace Syki.Shared;

public class TeacherClassActivityOut
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public ClassNoteType Note { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ClassActivityType Type { get; set; }
    public int Weight { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateOnly DueDate { get; set; }
    public Hour DueHour { get; set; }
    public int DeliveredWorks { get; set; }
    public int TotalWorks { get; set; }
    public List<ClassActivityWorkOut> Works { get; set; } = [];

    public string GetDueDate()
    {
        return $"{DueDate} {DueHour.GetDescription()}";
    }

    public decimal GetDeliveryRate()
    {
        if (TotalWorks == 0) return 0;
        return DeliveredWorks / (decimal)TotalWorks * 100;
    }
}
