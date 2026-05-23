namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Disciplines_CreateDiscipline_Should_not_create_discipline_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateDiscipline();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Disciplines_CreateDiscipline_Should_not_create_discipline_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateDiscipline();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    [TestCase("")]
    public async Task Disciplines_CreateDiscipline_Should_not_create_Discipline_with_invalid_name(string name)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.CreateDiscipline(name);

        // Assert
        response.ShouldBeError(InvalidDisciplineName.I);
    }
  
    #endregion

    #region Happy path

    [Test]
    public async Task Disciplines_CreateDiscipline_Should_create_Discipline()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateDiscipline("Física I");

        // Assert
        var discipline = result.Success;
        discipline.Id.Should().NotBe(0);
    }

    #endregion
}
