using Syki.Back.Features.Academic.CreateStudent;

namespace Syki.Tests.Features.Academic.CreateStudent;

public class CreateStudentUnitTests
{
    [Test]
    public void Should_create_student_with_correct_data()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        const string name = "Zaqueu";
        var courseOfferingId = Guid.NewGuid();

        // Act
        var student = new SykiStudent(userId, institutionId, "Zaqueu", courseOfferingId);

        // Assert
        student.Id.Should().NotBeEmpty();
        student.InstitutionId.Should().Be(institutionId);
        student.Id.Should().Be(userId);
        student.Name.Should().Be(name);
        student.CourseOfferingId.Should().Be(courseOfferingId);
        student.EnrollmentCode.Should().HaveLength(12);
        student.EnrollmentCode.Should().StartWith(DateTime.Now.Year.ToString());
        student.Status.Should().Be(StudentStatus.Enrolled);
    }

    [Test]
    [Repeat(100)]
    public void Should_create_students_with_distinct_enrollment_codes()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        var courseOfferingId = Guid.NewGuid();

        // Act
        var maria = new SykiStudent(userId, institutionId, "Maria", courseOfferingId);
        var zaqueu = new SykiStudent(userId, institutionId, "Zaqueu", courseOfferingId);

        // Assert
        maria.EnrollmentCode.Should().NotBeSameAs(zaqueu.EnrollmentCode);
    }

    [Test]
    public void Should_convert_student_to_out_without_course_offering()
    {
        // Arrange
        var student = new SykiStudent(Guid.NewGuid(), Guid.NewGuid(), "Zaqueu", Guid.NewGuid());

        // Act
        var studentOut = student.ToOut();

        // Assert
        studentOut.Id.Should().Be(student.Id);
        studentOut.CourseOfferingId.Should().Be(student.CourseOfferingId);
        studentOut.Name.Should().Be(student.Name);
        studentOut.CourseOffering.Should().Be("-");
        studentOut.EnrollmentCode.Should().Be(student.EnrollmentCode);
    }

    [Test]
    public void Should_convert_student_to_out_with_course_offering()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        var campusId = Guid.NewGuid();
        var courseId = Guid.NewGuid();
        var courseCurriculumId = Guid.NewGuid();
        const string period = "2024.1";
        var shift = Shift.Matutino;

        var student = new SykiStudent(userId, institutionId, "Zaqueu", Guid.NewGuid())
        {
            CourseOffering = new(institutionId, campusId, courseId, courseCurriculumId, period, shift)
            {
                Course = new(institutionId, "Direito", CourseType.Doutorado)
            }
        };

        // Act
        var studentOut = student.ToOut();

        // Assert
        studentOut.Id.Should().Be(student.Id);
        studentOut.CourseOfferingId.Should().Be(student.CourseOfferingId);
        studentOut.Name.Should().Be(student.Name);
        studentOut.CourseOffering.Should().Be("Direito");
        studentOut.EnrollmentCode.Should().Be(student.EnrollmentCode);
    }
}
