namespace Syki.Shared;

public class CreateStudentEnrollmentIn
{
    /// <summary>
    /// Turmas nas quais o aluno quer se matricular.
    /// </summary>
    public List<Guid> Classes { get; set; }
}
