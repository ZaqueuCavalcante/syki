namespace Syki.Shared;

public class StudentInsightsOut
{
    public StudentStatus Status { get; set; }

    public int FinishedDisciplines { get; set; }
    public int TotalDisciplines { get; set; }

    public decimal Average { get; set; }
    public decimal CR { get; set; }
}
