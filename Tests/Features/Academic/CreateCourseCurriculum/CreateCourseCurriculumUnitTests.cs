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

        var grade = new CourseCurriculum(institutionId, courseId, name)
        {
            Course = new Course(institutionId, "ADS", CourseType.Bacharelado),
            Disciplines = [
                new Discipline(institutionId, "Banco de Dados"),
                new Discipline(institutionId, "Estrutura de Dados"),
            ],
        };

        grade.Links.Add(new(grade.Disciplines[0].Id, 2, 12, 80));
        grade.Links.Add(new(grade.Disciplines[1].Id, 1, 8, 50));

        // Act
        var gradeOut = grade.ToOut();

        // Assert
        gradeOut.Id.Should().Be(grade.Id);
        gradeOut.CourseId.Should().Be(grade.CourseId);
        gradeOut.CourseName.Should().Be(grade.Course.Name);
        gradeOut.Name.Should().Be(grade.Name);

        gradeOut.Disciplines.Should().HaveCount(2);
        gradeOut.Disciplines[0].Name.Should().Be("Banco de Dados");
        gradeOut.Disciplines[0].Period.Should().Be(2);
        gradeOut.Disciplines[0].Credits.Should().Be(12);
        gradeOut.Disciplines[0].Workload.Should().Be(80);
        gradeOut.Disciplines[1].Name.Should().Be("Estrutura de Dados");
        gradeOut.Disciplines[1].Period.Should().Be(1);
        gradeOut.Disciplines[1].Credits.Should().Be(8);
        gradeOut.Disciplines[1].Workload.Should().Be(50);
    }
}
