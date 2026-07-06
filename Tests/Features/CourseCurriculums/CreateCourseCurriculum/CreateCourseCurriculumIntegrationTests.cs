using Syki.Back.Features.CourseCurriculums.CreateCourseCurriculum;

namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task CourseCurriculums_CreateCourseCurriculum_Should_not_create_course_curriculum_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateCourseCurriculum(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task CourseCurriculums_CreateCourseCurriculum_Should_not_create_course_curriculum_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateCourseCurriculum(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task CourseCurriculums_CreateCourseCurriculum_Should_not_create_course_curriculum_with_empty_name()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse();

        // Act
        var result = await client.CreateCourseCurriculum(course.Success.Id, "");

        // Assert
        result.ShouldBeError(InvalidCourseCurriculumName.I);
    }

    [Test]
    public async Task CourseCurriculums_CreateCourseCurriculum_Should_not_create_course_curriculum_with_too_long_name()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse();

        // Act
        var result = await client.CreateCourseCurriculum(course.Success.Id, new string('a', 51));

        // Assert
        result.ShouldBeError(InvalidCourseCurriculumName.I);
    }

    [Test]
    public async Task CourseCurriculums_CreateCourseCurriculum_Should_not_create_course_curriculum_with_course_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateCourseCurriculum(99999);

        // Assert
        result.ShouldBeError(CourseNotFound.I);
    }

    [Test]
    public async Task CourseCurriculums_CreateCourseCurriculum_Should_not_create_course_curriculum_with_disciplines_not_linked_to_course()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse();
        var discipline = await client.CreateDiscipline();

        List<CreateCourseCurriculumDisciplineIn> disciplines = [new(discipline.Success.Id, 1, 4, 72)];

        // Act
        var result = await client.CreateCourseCurriculum(course.Success.Id, "Grade 2024", disciplines);

        // Assert
        result.ShouldBeError(InvalidDisciplinesList.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task CourseCurriculums_CreateCourseCurriculum_Should_create_course_curriculum_without_disciplines()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse();

        // Act
        var result = await client.CreateCourseCurriculum(course.Success.Id, "Grade 2024");

        // Assert
        result.Success.Id.Should().BePositive();
    }

    [Test]
    public async Task CourseCurriculums_CreateCourseCurriculum_Should_create_course_curriculum_with_disciplines()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse();
        var discipline = await client.CreateDiscipline();
        await client.AddCourseDisciplines(course.Success.Id, [discipline.Success.Id]);

        List<CreateCourseCurriculumDisciplineIn> disciplines = [new(discipline.Success.Id, 1, 4, 72)];

        // Act
        var result = await client.CreateCourseCurriculum(course.Success.Id, "Grade 2024", disciplines);

        // Assert
        result.Success.Id.Should().BePositive();
    }

    #endregion
}
