namespace Syki.Shared;

public class StudentExamGradeOut
{
    public Guid ClassId { get; set; }
    public byte Period { get; set; }
    public string Discipline { get; set; }
    public StudentDisciplineStatus StudentDisciplineStatus { get; set; }
    public decimal AverageNote { get; set; }
    public List<ClassStudentNoteOut> ExamGrades { get; set; }

    public string GetNote(ClassStudentNoteType type)
    {
        var examGrade = ExamGrades.FirstOrDefault(x => x.ClassStudentNoteType == type);
        return examGrade != null ? examGrade.Note.Format() : "0.00";
    }
}
