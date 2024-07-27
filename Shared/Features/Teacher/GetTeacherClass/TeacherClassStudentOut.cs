namespace Syki.Shared;

public class TeacherClassStudentOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal AverageNote { get; set; }
    public List<ExamGradeOut> ExamGrades { get; set; } = [];

    public string GetNote(ExamType type)
    {
        return ExamGrades.Where(x => x.ExamType == type).First().Note.Format();
    }
}
