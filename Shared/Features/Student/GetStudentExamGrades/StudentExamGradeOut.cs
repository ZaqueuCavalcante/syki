namespace Syki.Shared;

public class StudentExamGradeOut
{
    public byte Period { get; set; }
    public string Discipline { get; set; }
    public StudentDisciplineStatus StudentDisciplineStatus { get; set; }
    public decimal AverageNote { get; set; }
    public List<ExamGradeOut> ExamGrades { get; set; }

    public string GetNote(ExamType type)
    {
        return ExamGrades.Where(x => x.ExamType == type).First().Note.Format();
    }
}
