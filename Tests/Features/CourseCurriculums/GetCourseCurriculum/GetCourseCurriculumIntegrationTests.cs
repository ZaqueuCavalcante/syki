using Estud.Back.Features.CourseCurriculums.CreateCourseCurriculum;

namespace Estud.Tests.Integration;

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
        var otherCourse = await otherClient.CreateCourse().Success();
        var otherCurriculum = await otherClient.CreateCourseCurriculum(otherCourse.Id).Success();

        // Act
        var result = await client.GetCourseCurriculum(otherCurriculum.Id);

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
        var course = await client.CreateCourse("ADS", CourseType.Tecnologo).Success();
        var curriculum = await client.CreateCourseCurriculum(course.Id, "Grade 2024").Success();

        // Act
        var result = await client.GetCourseCurriculum(curriculum.Id);

        // Assert
        var output = result.Success;
        output.Id.Should().Be(curriculum.Id);
        output.Name.Should().Be("Grade 2024");
        output.CourseId.Should().Be(course.Id);
        output.Course.Should().Be("ADS");
        output.Disciplines.Should().BeEmpty();
    }

    [Test]
    public async Task CourseCurriculums_GetCourseCurriculum_Should_get_course_curriculum_with_disciplines()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse("ADS", CourseType.Tecnologo).Success();
        var discipline1 = await client.CreateDiscipline("Cálculo I").Success();
        var discipline2 = await client.CreateDiscipline("Álgebra").Success();
        await client.AddCourseDisciplines(course.Id, [discipline1.Id, discipline2.Id]);

        List<CreateCourseCurriculumDisciplineIn> disciplines =
        [
            new(discipline1.Id, 2, 4, 72),
            new(discipline2.Id, 1, 4, 60),
        ];
        var curriculum = await client.CreateCourseCurriculum(course.Id, "Grade 2024", disciplines).Success();

        // Act
        var result = await client.GetCourseCurriculum(curriculum.Id);

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
