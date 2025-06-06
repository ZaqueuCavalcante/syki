using Syki.Back.Features.Academic.CreateCourseOffering;
using Syki.Back.Features.Academic.CreateCourseCurriculum;

namespace Syki.Tests.Features.Academic.CreateCourseOffering;

public class CreateCourseOfferingUnitTests
{
    [Test]
    public void Should_convert_course_offering_to_out()
    {
        // Arrange
        var institutionId = Guid.CreateVersion7();
        var campusId = Guid.CreateVersion7();
        var courseId = Guid.CreateVersion7();
        const string period = "2024.1";
        var shift = Shift.Matutino;

        var courseCurriculum = new CourseCurriculum(institutionId, courseId, "Grade de ADS - 1.0");

        var courseOffering = new CourseOffering(institutionId, campusId, courseId, courseCurriculum.Id, period, shift)
        {
            Campus = new(institutionId, "Agreste I", BrazilState.PE, "Caruaru"),
            Course = new(institutionId, "ADS", CourseType.Bacharelado),
            CourseCurriculum = courseCurriculum,
        };

        // Act
        var courseOfferingOut = courseOffering.ToOut();

        // Assert
        courseOfferingOut.Id.Should().Be(courseOffering.Id);
        courseOfferingOut.Campus.Should().Be(courseOffering.Campus.Name);
        courseOfferingOut.Course.Should().Be(courseOffering.Course.Name);
        courseOfferingOut.CourseCurriculumId.Should().Be(courseOffering.CourseCurriculumId);
        courseOfferingOut.CourseCurriculum.Should().Be(courseOffering.CourseCurriculum.Name);
        courseOfferingOut.Period.Should().Be(courseOffering.Period);
        courseOfferingOut.Shift.Should().Be(courseOffering.Shift);
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
