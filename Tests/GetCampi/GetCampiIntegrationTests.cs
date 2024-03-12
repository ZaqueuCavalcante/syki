namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    // [Test]
    public async Task Should_get_empty_list_when_has_no_campi()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        var campi = await client.GetAsync<List<GetCampusOut>>("/campi");

        // Assert
        campi.Should().BeEmpty();
    }

    // [Test]
    public async Task Should_get_many_campus()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        await client.CreateCampus("Agreste I", "Caruaru - PE");
        await client.CreateCampus("Suassuna I", "Recife - PE");

        // Act
        var campi = await client.GetAsync<List<GetCampusOut>>("/campi");

        // Assert
        campi.Should().HaveCount(2);
    }

    // [Test]
    public async Task Should_get_only_institution_campi()
    {
        // Arrange
        var client = _factory.CreateClient();
        var userNovaRoma = await client.NewAcademico("Nova Roma");
        var userUfpe = await client.NewAcademico("UFPE");

        await client.Login(userNovaRoma);
        await client.CreateCampus("Agreste I", "Caruaru - PE");

        await client.Login(userUfpe);
        await client.CreateCampus("Suassuna I", "Recife - PE");

        // Act
        await client.Login(userNovaRoma);

        // Assert
        var campi = await client.GetAsync<List<GetCampusOut>>("/campi");
        campi.Should().HaveCount(1);
        campi[0].Name.Should().Be("Agreste I");
        campi[0].City.Should().Be("Caruaru - PE");
    }
}
