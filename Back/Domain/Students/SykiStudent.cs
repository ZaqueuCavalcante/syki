using Syki.Back.Domain.Identity;

namespace Syki.Back.Domain.Students;

/// <summary>
/// Representa um Aluno.
/// </summary>
public class SykiStudent
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public string Name { get; set; }
    public string EnrollmentCode { get; set; }
    public StudentStatus Status { get; set; }
    public decimal YieldCoefficient { get; set; }

    public SykiUser? User { get; set; }

    private SykiStudent() {}

    public SykiStudent(
        int userId,
        int institutionId,
        string name
    ) {
        Id = userId;
        Name = name;
        InstitutionId = institutionId;
        Status = StudentStatus.Enrolled;
        EnrollmentCode = $"{DateTime.UtcNow.Year}{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }
}
