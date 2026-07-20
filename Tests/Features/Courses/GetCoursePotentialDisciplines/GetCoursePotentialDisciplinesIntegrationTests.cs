namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Courses_GetCoursePotentialDisciplines_Should_not_get_potential_disciplines_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetCoursePotentialDisciplines(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Courses_GetCoursePotentialDisciplines_Should_not_get_potential_disciplines_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetCoursePotentialDisciplines(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Courses_GetCoursePotentialDisciplines_Should_not_get_potential_disciplines_from_course_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetCoursePotentialDisciplines(99999);

        // Assert
        result.ShouldBeError(CourseNotFound.I);
    }

    [Test]
    public async Task Courses_GetCoursePotentialDisciplines_Should_not_get_potential_disciplines_from_other_institution_course()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var otherClient = await _back.LoggedAsDirector();
        var otherCourse = await otherClient.CreateCourse().Success();

        // Act
        var result = await client.GetCoursePotentialDisciplines(otherCourse.Id);

        // Assert
        result.ShouldBeError(CourseNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Courses_GetCoursePotentialDisciplines_Should_get_all_disciplines_when_none_linked()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse().Success();
        await client.CreateDiscipline("Geometria");
        await client.CreateDiscipline("Álgebra");

        // Act
        var result = await client.GetCoursePotentialDisciplines(course.Id);

        // Assert
        var output = result.Success;
        output.Items.Should().HaveCount(2);
        output.Items.First().Name.Should().Be("Álgebra");
        output.Items.Last().Name.Should().Be("Geometria");
    }

    [Test]
    public async Task Courses_GetCoursePotentialDisciplines_Should_not_get_already_linked_disciplines()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse().Success();
        var linkedDiscipline = await client.CreateDiscipline("Geometria").Success();
        var potentialDiscipline = await client.CreateDiscipline("Álgebra").Success();
        await client.AddCourseDisciplines(course.Id, [linkedDiscipline.Id]);

        // Act
        var result = await client.GetCoursePotentialDisciplines(course.Id);

        // Assert
        var output = result.Success;
        output.Items.Should().ContainSingle();
        output.Items.First().Id.Should().Be(potentialDiscipline.Id);
        output.Items.First().Name.Should().Be("Álgebra");
    }

    [Test]
    public async Task Courses_GetCoursePotentialDisciplines_Should_filter_potential_disciplines_by_name()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse().Success();
        await client.CreateDiscipline("Geometria");
        await client.CreateDiscipline("Álgebra");

        // Act
        var result = await client.GetCoursePotentialDisciplines(course.Id, "geo");

        // Assert
        var output = result.Success;
        output.Items.Should().ContainSingle();
        output.Items.First().Name.Should().Be("Geometria");
    }

    #endregion
}
