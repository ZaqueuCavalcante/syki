namespace Syki.Shared;

public class AcademicClassStudentOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal AverageNote { get; set; }
    public List<StudentClassNoteOut> Notes { get; set; } = [];
    public decimal Frequency { get; set; }

    public string GetNote(StudentClassNoteType type)
    {
        var note = Notes.FirstOrDefault(x => x.Type == type);
        return note != null ? note.Note.Format() : "0.00";
    }
}
