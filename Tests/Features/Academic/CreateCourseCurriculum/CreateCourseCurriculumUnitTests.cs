using Syki.Back.Features.Academic.CreateCourse;
using Syki.Back.Features.Academic.CreateDiscipline;
using Syki.Back.Features.Academic.CreateCourseCurriculum;

namespace Syki.Tests.Features.Academic.CreateCourseCurriculum;

public class CreateCourseCurriculumUnitTests
{
    [Test]
    public void Should_convert_course_curruculum_to_out()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var courseId = Guid.NewGuid();
        const string name = "Grade de ADS - 1.0";

        var cc = new CourseCurriculum(institutionId, courseId, name)
        {
            Course = new Course(institutionId, "ADS", CourseType.Bacharelado),
            Disciplines = [
                new Discipline(institutionId, "Banco de Dados"),
                new Discipline(institutionId, "Estrutura de Dados"),
            ],
        };

        cc.Links.Add(new(cc.Disciplines[0].Id, 2, 12, 80));
        cc.Links.Add(new(cc.Disciplines[1].Id, 1, 8, 50));

        // Act
        var ccOut = cc.ToOut();

        // Assert
        ccOut.Id.Should().Be(cc.Id);
        ccOut.CourseId.Should().Be(cc.CourseId);
        ccOut.CourseName.Should().Be(cc.Course.Name);
        ccOut.Name.Should().Be(cc.Name);

        ccOut.Disciplines.Should().HaveCount(2);
        ccOut.Disciplines[0].Name.Should().Be("Banco de Dados");
        ccOut.Disciplines[0].Period.Should().Be(2);
        ccOut.Disciplines[0].Credits.Should().Be(12);
        ccOut.Disciplines[0].Workload.Should().Be(80);
        ccOut.Disciplines[1].Name.Should().Be("Estrutura de Dados");
        ccOut.Disciplines[1].Period.Should().Be(1);
        ccOut.Disciplines[1].Credits.Should().Be(8);
        ccOut.Disciplines[1].Workload.Should().Be(50);
    }
}
