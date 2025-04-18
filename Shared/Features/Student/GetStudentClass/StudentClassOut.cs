namespace Syki.Shared;

public class StudentClassOut
{
    public Guid Id { get; set; }
    public string Discipline { get; set; }
    public string Code { get; set; }
    public string Period { get; set; }
    public ClassStatus Status { get; set; }
    public decimal N1 { get; set; }
    public decimal N2 { get; set; }
    public decimal N3 { get; set; }
    public decimal Average { get; set; }
}
