using Estud.Back.Domain.Identity;

namespace Estud.Back.Domain.Students;

/// <summary>
/// Representa um Aluno.
/// </summary>
public class EstudStudent
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string EnrollmentCode { get; set; }
    public StudentStatus Status { get; set; }
    public decimal YieldCoefficient { get; set; }

    public EstudUser? User { get; set; }

    private EstudStudent() {}

    public EstudStudent(
        EstudUser user,
        int institutionId,
        string name
    ) {
        User = user;
        Name = name;
        InstitutionId = institutionId;
        Status = StudentStatus.Enrolled;
        EnrollmentCode = $"{DateTime.Now.Year}{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }
}
