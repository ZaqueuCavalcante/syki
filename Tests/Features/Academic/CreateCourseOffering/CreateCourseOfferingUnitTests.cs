using Syki.Back.Features.Academic.CreateCourseOffering;
using Syki.Back.Features.Academic.CreateCourseCurriculum;

namespace Syki.Tests.Unit;

public class CreateCourseOfferingUnitTests
{
    [Test]
    public void Should_convert_course_offering_to_out()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var campusId = Guid.NewGuid();
        var courseId = Guid.NewGuid();
        const string period = "2024.1";
        var shift = Shift.Matutino;

        var grade = new CourseCurriculum(institutionId, courseId, "Grade de ADS - 1.0");

        var oferta = new CourseOffering(institutionId, campusId, courseId, grade.Id, period, shift)
        {
            Campus = new(institutionId, "Agreste I", "Caruaru - PE"),
            Course = new(institutionId, "ADS", CourseType.Bacharelado),
            CourseCurriculum = grade,
        };

        // Act
        var courseOfferingOut = oferta.ToOut();

        // Assert
        courseOfferingOut.Id.Should().Be(oferta.Id);
        courseOfferingOut.Campus.Should().Be(oferta.Campus.Name);
        courseOfferingOut.Course.Should().Be(oferta.Course.Name);
        courseOfferingOut.CourseCurriculumId.Should().Be(oferta.CourseCurriculumId);
        courseOfferingOut.CourseCurriculum.Should().Be(oferta.CourseCurriculum.Name);
        courseOfferingOut.Period.Should().Be(oferta.Period);
        courseOfferingOut.Shift.Should().Be(oferta.Shift);
    }

    [Test]
    public void Should_convert_course_offering_to_string()
    {
        // Arrange
        var courseOfferingOut = new CourseOfferingOut
        {
            CourseCurriculum = "Grade de ADS - 1.0",
            Campus = "Agreste I",
            Period = "2024.1",
            Shift = Shift.Matutino,
        };

        // Act
        var getLine = courseOfferingOut.ToString();

        // Assert
        getLine.Should().Be("Grade de ADS - 1.0 | Agreste I | 2024.1 | Matutino");
    }
}
