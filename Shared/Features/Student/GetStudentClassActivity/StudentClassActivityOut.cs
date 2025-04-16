namespace Syki.Shared;

public class StudentClassActivityOut
{
    public Guid Id { get; set; }
    public ClassNoteType Note { get; set; }
    public string Title { get; set; }
    public ClassActivityType Type { get; set; }
    public ClassActivityStatus Status { get; set; }
    public int Weight { get; set; }
    public decimal Value { get; set; }
    public decimal PonderedValue { get; set; }
    public ClassActivityWorkStatus WorkStatus { get; set; }
}
