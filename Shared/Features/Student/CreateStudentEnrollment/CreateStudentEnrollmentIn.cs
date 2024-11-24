namespace Syki.Shared;

public class CreateStudentEnrollmentIn
{
    /// <summary>
    /// Turmas nas quais o aluno quer se matricular.
    /// </summary>
    /// <example>
    /// <code>["0638fe4b-1cd4-463b-9037-609727620a37", "92d7510e-fb9d-4a79-9d46-49cf8de363aa"]</code>
    /// </example>
    public List<Guid> Classes { get; set; }
}
