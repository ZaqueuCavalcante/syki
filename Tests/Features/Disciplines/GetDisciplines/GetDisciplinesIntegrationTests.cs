namespace Estud.Tests.Integration;

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
        disciplines.Page.Should().Be(1);
        disciplines.PageSize.Should().Be(10);
        disciplines.Items.First().Name.Should().Be("Física I");
        disciplines.Items.Last().Name.Should().Be("Química I");
    }

    [Test]
    public async Task Disciplines_GetDisciplines_Should_get_only_the_first_10_disciplines_by_default()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        for (var i = 1; i <= 12; i++)
            await client.CreateDiscipline($"Disciplina {i:00}");

        // Act
        var result = await client.GetDisciplines();

        // Assert
        var disciplines = result.Success;
        disciplines.Total.Should().Be(12);
        disciplines.Page.Should().Be(1);
        disciplines.PageSize.Should().Be(10);
        disciplines.Items.Should().HaveCount(10);
        disciplines.Items.First().Name.Should().Be("Disciplina 01");
        disciplines.Items.Last().Name.Should().Be("Disciplina 10");
    }

    [Test]
    public async Task Disciplines_GetDisciplines_Should_get_disciplines_from_the_second_page()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        for (var i = 1; i <= 12; i++)
            await client.CreateDiscipline($"Disciplina {i:00}");

        // Act
        var result = await client.GetDisciplines(page: 2);

        // Assert
        var disciplines = result.Success;
        disciplines.Total.Should().Be(12);
        disciplines.Page.Should().Be(2);
        disciplines.Items.Should().HaveCount(2);
        disciplines.Items.First().Name.Should().Be("Disciplina 11");
        disciplines.Items.Last().Name.Should().Be("Disciplina 12");
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
