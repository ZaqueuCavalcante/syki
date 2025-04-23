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
        InstitutionId = institutionId;
        CourseOfferingId = courseOfferingId;
        Name = name;
        EnrollmentCode = $"{DateTime.Now.Year}{Guid.NewGuid().ToString()[..8].ToUpper()}";
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

    public TeacherClassStudentOut ToTeacherClassStudentOut()
    {
        return new()
        {
            Id = Id,
            Name = Name,
            Notes = [],
        };
    }

    public AcademicClassStudentOut ToAcademicClassStudentOut()
    {
        return new()
        {
            Id = Id,
            Name = Name,
            Notes = [],
        };
    }
}
