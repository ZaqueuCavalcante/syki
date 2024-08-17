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
    public string Name { get; }
    public string EnrollmentCode { get; }
    public StudentStatus Status { get; }

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
        Status = StudentStatus.Enrolled;
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
            ExamGrades = [],
        };
    }

    public AcademicClassStudentOut ToAcademicClassStudentOut()
    {
        return new()
        {
            Id = Id,
            Name = Name,
            ExamGrades = [],
        };
    }
}
