using Syki.Back.Features.Cross.CreateUser;
using Syki.Back.Features.Academic.CreateCourseOffering;

namespace Syki.Back.Features.Academic.CreateStudent;

/// <summary>
/// Representa um Aluno.
/// </summary>
public class SykiStudent : Entity
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public SykiUser User { get; set; }
    public Guid CourseOfferingId { get; set; }
    public CourseOffering CourseOffering { get; set; }
    public string Name { get; set; }
    public string EnrollmentCode { get; set; }
    public StudentStatus Status { get; set; }
    public decimal YieldCoefficient { get; set; }

    private SykiStudent() {}

    public SykiStudent(
        Guid userId,
        Guid institutionId,
        string name,
        Guid courseOfferingId
    ) {
        Id = userId;
        Name = name;
        InstitutionId = institutionId;
        CourseOfferingId = courseOfferingId;
        EnrollmentCode = $"{DateTime.UtcNow.Year}{Guid.CreateVersion7().ToString()[^8..].ToUpper()}";
        Status = StudentStatus.Enrolled;

        AddDomainEvent(new StudentCreatedDomainEvent(Id, InstitutionId));
    }

    public StudentOut ToOut()
    {
        return new()
        {
            Id = Id,
            Name = Name,
            Email = User?.Email ?? "-",
            PhoneNumber = User?.PhoneNumber,
            EnrollmentCode = EnrollmentCode,
            CourseOfferingId = CourseOfferingId,
            CourseOffering = CourseOffering?.Course?.Name ?? "-",
        };
    }
}
