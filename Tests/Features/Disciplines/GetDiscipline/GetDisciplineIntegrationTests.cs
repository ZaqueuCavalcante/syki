namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Disciplines_GetDiscipline_Should_not_get_discipline_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetDiscipline(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Disciplines_GetDiscipline_Should_not_get_discipline_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetDiscipline(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Disciplines_GetDiscipline_Should_not_get_discipline_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.GetDiscipline(99999);

        // Assert
        response.ShouldBeError(DisciplineNotFound.I);
    }

    [Test]
    public async Task Disciplines_GetDiscipline_Should_not_get_other_institution_discipline()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var otherClient = await _back.LoggedAsDirector();
        var otherDiscipline = await otherClient.CreateDiscipline().Success();

        // Act
        var response = await client.GetDiscipline(otherDiscipline.Id);

        // Assert
        response.ShouldBeError(DisciplineNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Disciplines_GetDiscipline_Should_get_discipline_without_courses()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline("Cálculo I").Success();

        // Act
        var result = await client.GetDiscipline(discipline.Id);

        // Assert
        var output = result.Success;
        output.Id.Should().Be(discipline.Id);
        output.Name.Should().Be("Cálculo I");
        output.Code.Should().NotBeEmpty();
        output.Courses.Should().BeEmpty();
    }

    [Test]
    public async Task Disciplines_GetDiscipline_Should_get_discipline_with_courses()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline("Cálculo I").Success();
        var course1 = await client.CreateCourse("Engenharia", CourseType.Bacharelado).Success();
        var course2 = await client.CreateCourse("ADS", CourseType.Tecnologo).Success();
        await client.AddDisciplineCourses(discipline.Id, [course1.Id, course2.Id]);

        // Act
        var result = await client.GetDiscipline(discipline.Id);

        // Assert
        var output = result.Success;
        output.Courses.Should().HaveCount(2);
        output.Courses.First().Name.Should().Be("ADS");
        output.Courses.Last().Name.Should().Be("Engenharia");
    }

    #endregion
}
