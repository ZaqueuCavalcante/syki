namespace Syki.Shared;

public class TeacherClassStudentOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal AverageNote { get; set; }
    public List<ClassStudentNoteOut> ExamGrades { get; set; } = [];

    public bool IsSelected { get; set; }

    public string GetNote(ClassStudentNoteType type)
    {
        return ExamGrades.Where(x => x.ClassStudentNoteType == type).First().Note.Format();
    }
}
