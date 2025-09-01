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
    public DateOnly? DateOfBirth { get; set; }
    public int PhoneNumber { get; set; }
    public string EnrollmentCode { get; set; }
    public StudentStatus Status { get; set; }
    public decimal YieldCoefficient { get; set; }

    private SykiStudent() {}
    //TODO: Implementar construtor completo
    public SykiStudent(
        Guid userId,
        Guid institutionId,
        string name,
        Guid courseOfferingId,
        DateOnly? dateOfBirth,
        int phoneNumber
    ) {
        Id = userId;
        Name = name;
        InstitutionId = institutionId;
        CourseOfferingId = courseOfferingId;
        DateOfBirth = dateOfBirth;
        EnrollmentCode = $"{DateTime.UtcNow.Year}{Guid.NewGuid().ToString()[..8].ToUpper()}";
        Status = StudentStatus.Enrolled;

        AddDomainEvent(new StudentCreatedDomainEvent(Id, InstitutionId));
    }

    public SykiStudent(Guid userId, Guid institutionId, string name, Guid courseOfferingId, DateOnly? dateOfBirth) : this(userId, institutionId, name, courseOfferingId, dateOfBirth, 0)
    {
    }

    public SykiStudent(Guid userId, Guid institutionId, string name, Guid courseOfferingId) : this(userId, institutionId, name, courseOfferingId, null, 0)
    {
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
