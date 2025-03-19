namespace Syki.Shared;

public class ClassStudentNoteOut
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public Guid StudentId { get; set; }
    public ClassStudentNoteType ClassStudentNoteType { get; set; }
    public decimal Note { get; set; }
}
