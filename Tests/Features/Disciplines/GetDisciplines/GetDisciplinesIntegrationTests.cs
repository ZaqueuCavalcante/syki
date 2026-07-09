namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Disciplines_GetDisciplines_Should_not_get_disciplines_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetDisciplines();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Disciplines_GetDisciplines_Should_not_get_disciplines_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetDisciplines();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Disciplines_GetDisciplines_Should_get_disciplines()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        await client.CreateDiscipline("Química I");
        await client.CreateDiscipline("Física I");

        // Act
        var result = await client.GetDisciplines();

        // Assert
        var disciplines = result.Success;
        disciplines.Total.Should().Be(2);
        disciplines.Items.First().Name.Should().Be("Física I");
        disciplines.Items.Last().Name.Should().Be("Química I");
    }

    [Test]
    public async Task Disciplines_GetDisciplines_Should_get_disciplines_filtered_by_text()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        await client.CreateDiscipline("Química I");
        await client.CreateDiscipline("Física I");

        // Act
        var result = await client.GetDisciplines("quím");

        // Assert
        var disciplines = result.Success;
        disciplines.Total.Should().Be(1);
        disciplines.Items.Single().Name.Should().Be("Química I");
    }

    #endregion
}
