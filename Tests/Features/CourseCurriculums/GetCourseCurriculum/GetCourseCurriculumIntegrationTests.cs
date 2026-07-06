using Syki.Back.Features.CourseCurriculums.CreateCourseCurriculum;

namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task CourseCurriculums_GetCourseCurriculum_Should_not_get_course_curriculum_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetCourseCurriculum(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task CourseCurriculums_GetCourseCurriculum_Should_not_get_course_curriculum_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetCourseCurriculum(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task CourseCurriculums_GetCourseCurriculum_Should_not_get_course_curriculum_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetCourseCurriculum(99999);

        // Assert
        result.ShouldBeError(CourseCurriculumNotFound.I);
    }

    [Test]
    public async Task CourseCurriculums_GetCourseCurriculum_Should_not_get_other_institution_course_curriculum()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var otherClient = await _back.LoggedAsDirector();
        var otherCourse = await otherClient.CreateCourse();
        var otherCurriculum = await otherClient.CreateCourseCurriculum(otherCourse.Success.Id);

        // Act
        var result = await client.GetCourseCurriculum(otherCurriculum.Success.Id);

        // Assert
        result.ShouldBeError(CourseCurriculumNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task CourseCurriculums_GetCourseCurriculum_Should_get_course_curriculum_without_disciplines()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse("ADS", CourseType.Tecnologo);
        var curriculum = await client.CreateCourseCurriculum(course.Success.Id, "Grade 2024");

        // Act
        var result = await client.GetCourseCurriculum(curriculum.Success.Id);

        // Assert
        var output = result.Success;
        output.Id.Should().Be(curriculum.Success.Id);
        output.Name.Should().Be("Grade 2024");
        output.CourseId.Should().Be(course.Success.Id);
        output.Course.Should().Be("ADS");
        output.Disciplines.Should().BeEmpty();
    }

    [Test]
    public async Task CourseCurriculums_GetCourseCurriculum_Should_get_course_curriculum_with_disciplines()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse("ADS", CourseType.Tecnologo);
        var discipline1 = await client.CreateDiscipline("Cálculo I");
        var discipline2 = await client.CreateDiscipline("Álgebra");
        await client.AddCourseDisciplines(course.Success.Id, [discipline1.Success.Id, discipline2.Success.Id]);

        List<CreateCourseCurriculumDisciplineIn> disciplines =
        [
            new(discipline1.Success.Id, 2, 4, 72),
            new(discipline2.Success.Id, 1, 4, 60),
        ];
        var curriculum = await client.CreateCourseCurriculum(course.Success.Id, "Grade 2024", disciplines);

        // Act
        var result = await client.GetCourseCurriculum(curriculum.Success.Id);

        // Assert
        var output = result.Success;
        output.Disciplines.Should().HaveCount(2);
        output.Disciplines.First().Name.Should().Be("Álgebra");
        output.Disciplines.First().Period.Should().Be(1);
        output.Disciplines.Last().Name.Should().Be("Cálculo I");
        output.Disciplines.Last().Period.Should().Be(2);
    }

    #endregion
}
