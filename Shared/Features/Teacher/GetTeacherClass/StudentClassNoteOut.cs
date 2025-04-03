namespace Syki.Shared;

public class StudentClassNoteOut
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public Guid StudentId { get; set; }
    public ClassNoteType Type { get; set; }
    public decimal Note { get; set; }
}
