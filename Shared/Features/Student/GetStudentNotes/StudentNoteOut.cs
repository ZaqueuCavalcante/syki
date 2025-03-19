namespace Syki.Shared;

public class StudentNoteOut
{
    public Guid ClassId { get; set; }
    public byte Period { get; set; }
    public string Discipline { get; set; }
    public StudentDisciplineStatus StudentDisciplineStatus { get; set; }
    public decimal AverageNote { get; set; }
    public List<StudentClassNoteOut> Notes { get; set; }

    public string GetNote(StudentClassNoteType type)
    {
        var note = Notes.FirstOrDefault(x => x.Type == type);
        return note != null ? note.Note.Format() : "0.00";
    }
}
