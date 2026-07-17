using Estud.Back.Features.CourseCurriculums.UpdateCourseCurriculum;

namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task CourseCurriculums_UpdateCourseCurriculum_Should_not_update_course_curriculum_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.UpdateCourseCurriculum(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task CourseCurriculums_UpdateCourseCurriculum_Should_not_update_course_curriculum_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.UpdateCourseCurriculum(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task CourseCurriculums_UpdateCourseCurriculum_Should_not_update_course_curriculum_with_empty_name()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse();
        var curriculum = await client.CreateCourseCurriculum(course.Success.Id);

        // Act
        var result = await client.UpdateCourseCurriculum(curriculum.Success.Id, "");

        // Assert
        result.ShouldBeError(InvalidCourseCurriculumName.I);
    }

    [Test]
    public async Task CourseCurriculums_UpdateCourseCurriculum_Should_not_update_course_curriculum_with_too_long_name()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse();
        var curriculum = await client.CreateCourseCurriculum(course.Success.Id);

        // Act
        var result = await client.UpdateCourseCurriculum(curriculum.Success.Id, new string('a', 51));

        // Assert
        result.ShouldBeError(InvalidCourseCurriculumName.I);
    }

    [Test]
    public async Task CourseCurriculums_UpdateCourseCurriculum_Should_not_update_course_curriculum_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateCourseCurriculum(99999);

        // Assert
        result.ShouldBeError(CourseCurriculumNotFound.I);
    }

    [Test]
    public async Task CourseCurriculums_UpdateCourseCurriculum_Should_not_update_other_institution_course_curriculum()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var otherClient = await _back.LoggedAsDirector();
        var otherCourse = await otherClient.CreateCourse();
        var otherCurriculum = await otherClient.CreateCourseCurriculum(otherCourse.Success.Id);

        // Act
        var result = await client.UpdateCourseCurriculum(otherCurriculum.Success.Id);

        // Assert
        result.ShouldBeError(CourseCurriculumNotFound.I);
    }

    [Test]
    public async Task CourseCurriculums_UpdateCourseCurriculum_Should_not_update_course_curriculum_with_course_offering()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = (await client.CreateCampus()).Success;
        var course = (await client.CreateCourse()).Success;
        var curriculum = (await client.CreateCourseCurriculum(course.Id)).Success;
        var period = (await client.CreateAcademicPeriod("2024.1")).Success;
        await client.CreateCourseOffering(campus.Id, course.Id, curriculum.Id, period.Id);

        // Act
        var result = await client.UpdateCourseCurriculum(curriculum.Id, "Grade 2025");

        // Assert
        result.ShouldBeError(CourseCurriculumWithCourseOffering.I);
    }

    [Test]
    public async Task CourseCurriculums_UpdateCourseCurriculum_Should_not_update_course_curriculum_with_disciplines_not_linked_to_course()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse();
        var curriculum = await client.CreateCourseCurriculum(course.Success.Id);
        var discipline = await client.CreateDiscipline();

        List<UpdateCourseCurriculumDisciplineIn> disciplines =
            [new() { Id = discipline.Success.Id, Period = 1, Credits = 4, Workload = 72 }];

        // Act
        var result = await client.UpdateCourseCurriculum(curriculum.Success.Id, "Grade 2024", disciplines);

        // Assert
        result.ShouldBeError(InvalidDisciplinesList.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task CourseCurriculums_UpdateCourseCurriculum_Should_update_course_curriculum_without_disciplines()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse();
        var curriculum = await client.CreateCourseCurriculum(course.Success.Id);

        // Act
        var result = await client.UpdateCourseCurriculum(curriculum.Success.Id, "Grade 2025");

        // Assert
        result.ShouldBeSuccess();
    }

    [Test]
    public async Task CourseCurriculums_UpdateCourseCurriculum_Should_update_course_curriculum_with_disciplines()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse();
        var curriculum = await client.CreateCourseCurriculum(course.Success.Id);
        var discipline = await client.CreateDiscipline();
        await client.AddCourseDisciplines(course.Success.Id, [discipline.Success.Id]);

        List<UpdateCourseCurriculumDisciplineIn> disciplines =
            [new() { Id = discipline.Success.Id, Period = 1, Credits = 4, Workload = 72 }];

        // Act
        var result = await client.UpdateCourseCurriculum(curriculum.Success.Id, "Grade 2025", disciplines);

        // Assert
        result.ShouldBeSuccess();
    }

    #endregion
}
