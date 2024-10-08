namespace Syki.Shared;

public class AcademicClassStudentOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal AverageNote { get; set; }
    public List<ExamGradeOut> ExamGrades { get; set; } = [];
    public decimal Frequency { get; set; }

    public string GetNote(ExamType type)
    {
        var examGrade = ExamGrades.FirstOrDefault(x => x.ExamType == type);
        return examGrade != null ? examGrade.Note.Format() : "0.00";
    }
}
