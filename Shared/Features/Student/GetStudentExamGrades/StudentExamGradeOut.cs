namespace Syki.Shared;

public class StudentExamGradeOut
{
    public string Discipline { get; set; }
    public byte Period { get; set; }
    public StudentDisciplineStatus StudentDisciplineStatus { get; set; }

    public decimal N1Note { get; set; }
    public decimal N2Note { get; set; }
    public decimal FinalNote { get; set; }
    public decimal AverageNote { get; set; }
}
