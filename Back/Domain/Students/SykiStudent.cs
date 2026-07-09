using Syki.Back.Domain.Identity;

namespace Syki.Back.Domain.Students;

/// <summary>
/// Representa um Aluno.
/// </summary>
public class SykiStudent
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string EnrollmentCode { get; set; }
    public StudentStatus Status { get; set; }
    public decimal YieldCoefficient { get; set; }

    public SykiUser? User { get; set; }

    private SykiStudent() {}

    public SykiStudent(
        SykiUser user,
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
