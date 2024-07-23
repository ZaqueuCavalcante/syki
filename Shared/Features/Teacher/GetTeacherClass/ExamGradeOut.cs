namespace Syki.Shared;

public class ExamGradeOut
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public Guid StudentId { get; set; }
    public ExamType ExamType { get; set; }
    public decimal Note { get; set; }
}
