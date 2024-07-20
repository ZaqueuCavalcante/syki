using Syki.Back.Features.Cross.CreateUser;
using Syki.Back.Features.Academic.CreateCourseOffering;

namespace Syki.Back.Features.Academic.CreateStudent;

public class SykiStudent
{
    public Guid Id { get; }
    public Guid InstitutionId { get; }
    public SykiUser User { get; }
    public Guid CourseOfferingId { get; }
    public CourseOffering CourseOffering { get; set; }
    public string Name { get; private set; }
    public string EnrollmentCode { get; }

    public SykiStudent(
        Guid id,
        Guid institutionId,
        string name,
        Guid courseOfferingId
    ) {
        Id = id;
        InstitutionId = institutionId;
        CourseOfferingId = courseOfferingId;
        Name = name;
        EnrollmentCode = $"{DateTime.Now.Year}{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }

    public StudentOut ToOut()
    {
        return new StudentOut
        {
            Id = Id,
            CourseOfferingId = CourseOfferingId,
            CourseOffering = CourseOffering?.Course?.Name ?? "-",
            Email = User?.Email ?? "-",
            Name = Name,
            EnrollmentCode = EnrollmentCode,
        };
    }
}
