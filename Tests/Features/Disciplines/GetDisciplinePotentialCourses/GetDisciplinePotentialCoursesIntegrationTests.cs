namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Disciplines_GetDisciplinePotentialCourses_Should_not_get_potential_courses_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetDisciplinePotentialCourses(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Disciplines_GetDisciplinePotentialCourses_Should_not_get_potential_courses_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetDisciplinePotentialCourses(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Disciplines_GetDisciplinePotentialCourses_Should_not_get_potential_courses_from_discipline_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetDisciplinePotentialCourses(99999);

        // Assert
        result.ShouldBeError(DisciplineNotFound.I);
    }

    [Test]
    public async Task Disciplines_GetDisciplinePotentialCourses_Should_not_get_potential_courses_from_other_institution_discipline()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var otherClient = await _back.LoggedAsDirector();
        var otherDiscipline = await otherClient.CreateDiscipline().Success();

        // Act
        var result = await client.GetDisciplinePotentialCourses(otherDiscipline.Id);

        // Assert
        result.ShouldBeError(DisciplineNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Disciplines_GetDisciplinePotentialCourses_Should_get_all_courses_when_none_linked()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        await client.CreateCourse("ADS", CourseType.Tecnologo);
        await client.CreateCourse("Engenharia", CourseType.Bacharelado);

        // Act
        var result = await client.GetDisciplinePotentialCourses(discipline.Id);

        // Assert
        var output = result.Success;
        output.Items.Should().HaveCount(2);
        output.Items.First().Name.Should().Be("ADS");
        output.Items.Last().Name.Should().Be("Engenharia");
    }

    [Test]
    public async Task Disciplines_GetDisciplinePotentialCourses_Should_not_get_already_linked_courses()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var linkedCourse = await client.CreateCourse("ADS", CourseType.Tecnologo).Success();
        var potentialCourse = await client.CreateCourse("Engenharia", CourseType.Bacharelado).Success();
        await client.AddDisciplineCourses(discipline.Id, [linkedCourse.Id]);

        // Act
        var result = await client.GetDisciplinePotentialCourses(discipline.Id);

        // Assert
        var output = result.Success;
        output.Items.Should().ContainSingle();
        output.Items.First().Id.Should().Be(potentialCourse.Id);
        output.Items.First().Name.Should().Be("Engenharia");
    }

    [Test]
    public async Task Disciplines_GetDisciplinePotentialCourses_Should_filter_potential_courses_by_name()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        await client.CreateCourse("ADS", CourseType.Tecnologo);
        await client.CreateCourse("Engenharia", CourseType.Bacharelado);

        // Act
        var result = await client.GetDisciplinePotentialCourses(discipline.Id, "eng");

        // Assert
        var output = result.Success;
        output.Items.Should().ContainSingle();
        output.Items.First().Name.Should().Be("Engenharia");
    }

    #endregion
}
