namespace Syki.Shared;

public class TeacherClassStudentOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal AverageNote { get; set; }
    public List<StudentClassNoteOut> Notes { get; set; } = [];

    public string GetNote(ClassNoteType type)
    {
        return Notes.Where(x => x.Type == type).First().Note.Format();
    }
}
