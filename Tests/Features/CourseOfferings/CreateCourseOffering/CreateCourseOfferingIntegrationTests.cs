namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task CourseOfferings_CreateCourseOffering_Should_not_create_course_offering_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateCourseOffering(1, 1, 1, 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task CourseOfferings_CreateCourseOffering_Should_not_create_course_offering_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateCourseOffering(1, 1, 1, 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task CourseOfferings_CreateCourseOffering_Should_not_create_course_offering_with_invalid_session()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateCourseOffering(1, 1, 1, 1, (CourseSession)99);

        // Assert
        result.ShouldBeError(InvalidCourseSession.I);
    }

    [Test]
    public async Task CourseOfferings_CreateCourseOffering_Should_not_create_course_offering_with_campus_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateCourseOffering(99999, 1, 1, 1);

        // Assert
        result.ShouldBeError(CampusNotFound.I);
    }

    [Test]
    public async Task CourseOfferings_CreateCourseOffering_Should_not_create_course_offering_with_course_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = (await client.CreateCampus()).Success;

        // Act
        var result = await client.CreateCourseOffering(campus.Id, 99999, 1, 1);

        // Assert
        result.ShouldBeError(CourseNotFound.I);
    }

    [Test]
    public async Task CourseOfferings_CreateCourseOffering_Should_not_create_course_offering_with_course_curriculum_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = (await client.CreateCampus()).Success;
        var course = (await client.CreateCourse()).Success;

        // Act
        var result = await client.CreateCourseOffering(campus.Id, course.Id, 99999, 1);

        // Assert
        result.ShouldBeError(CourseCurriculumNotFound.I);
    }

    [Test]
    public async Task CourseOfferings_CreateCourseOffering_Should_not_create_course_offering_with_academic_period_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = (await client.CreateCampus()).Success;
        var course = (await client.CreateCourse()).Success;
        var curriculum = (await client.CreateCourseCurriculum(course.Id)).Success;

        // Act
        var result = await client.CreateCourseOffering(campus.Id, course.Id, curriculum.Id, 99999);

        // Assert
        result.ShouldBeError(AcademicPeriodNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task CourseOfferings_CreateCourseOffering_Should_create_course_offering()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = (await client.CreateCampus()).Success;
        var course = (await client.CreateCourse()).Success;
        var curriculum = (await client.CreateCourseCurriculum(course.Id)).Success;
        var period = (await client.CreateAcademicPeriod("2024.1")).Success;

        // Act
        var result = await client.CreateCourseOffering(campus.Id, course.Id, curriculum.Id, period.Id);

        // Assert
        result.Success.Id.Should().BePositive();
    }

    #endregion
}
