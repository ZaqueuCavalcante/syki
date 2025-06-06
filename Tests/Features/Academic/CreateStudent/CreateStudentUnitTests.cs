using Syki.Back.Features.Academic.CreateStudent;

namespace Syki.Tests.Features.Academic.CreateStudent;

public class CreateStudentUnitTests
{
    [Test]
    public void Should_create_student_with_correct_data()
    {
        // Arrange
        var userId = Guid.CreateVersion7();
        var institutionId = Guid.CreateVersion7();
        const string name = "Zaqueu";
        var courseOfferingId = Guid.CreateVersion7();

        // Act
        var student = new SykiStudent(userId, institutionId, "Zaqueu", courseOfferingId);

        // Assert
        student.Id.Should().NotBeEmpty();
        student.InstitutionId.Should().Be(institutionId);
        student.Id.Should().Be(userId);
        student.Name.Should().Be(name);
        student.CourseOfferingId.Should().Be(courseOfferingId);
        student.EnrollmentCode.Should().HaveLength(12);
        student.EnrollmentCode.Should().StartWith(DateTime.UtcNow.Year.ToString());
        student.Status.Should().Be(StudentStatus.Enrolled);
    }

    [Test]
    [Repeat(100)]
    public void Should_create_students_with_distinct_enrollment_codes()
    {
        // Arrange
        var userId = Guid.CreateVersion7();
        var institutionId = Guid.CreateVersion7();
        var courseOfferingId = Guid.CreateVersion7();

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
        var student = new SykiStudent(Guid.CreateVersion7(), Guid.CreateVersion7(), "Zaqueu", Guid.CreateVersion7());

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
        var userId = Guid.CreateVersion7();
        var institutionId = Guid.CreateVersion7();
        var campusId = Guid.CreateVersion7();
        var courseId = Guid.CreateVersion7();
        var courseCurriculumId = Guid.CreateVersion7();
        const string period = "2024.1";
        var shift = Shift.Matutino;

        var student = new SykiStudent(userId, institutionId, "Zaqueu", Guid.CreateVersion7())
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
