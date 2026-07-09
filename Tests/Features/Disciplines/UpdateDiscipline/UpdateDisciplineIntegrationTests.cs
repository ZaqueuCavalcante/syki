namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Disciplines_UpdateDiscipline_Should_not_update_discipline_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.UpdateDiscipline(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Disciplines_UpdateDiscipline_Should_not_update_discipline_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.UpdateDiscipline(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    [TestCase("")]
    public async Task Disciplines_UpdateDiscipline_Should_not_update_discipline_with_invalid_name(string name)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline();

        // Act
        var response = await client.UpdateDiscipline(discipline.Success.Id, name);

        // Assert
        response.ShouldBeError(InvalidDisciplineName.I);
    }

    [Test]
    public async Task Disciplines_UpdateDiscipline_Should_not_update_discipline_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.UpdateDiscipline(99999);

        // Assert
        response.ShouldBeError(DisciplineNotFound.I);
    }

    [Test]
    public async Task Disciplines_UpdateDiscipline_Should_not_update_other_institution_discipline()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var otherClient = await _back.LoggedAsDirector();
        var otherDiscipline = await otherClient.CreateDiscipline();

        // Act
        var response = await client.UpdateDiscipline(otherDiscipline.Success.Id);

        // Assert
        response.ShouldBeError(DisciplineNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Disciplines_UpdateDiscipline_Should_update_discipline()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline("Física I");

        // Act
        var result = await client.UpdateDiscipline(discipline.Success.Id, "Física II");

        // Assert
        var updated = result.Success;
        updated.Id.Should().Be(discipline.Success.Id);
        updated.Name.Should().Be("Física II");
    }

    #endregion
}
