namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_get_empty_list_when_has_no_campi()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        var campi = await client.GetAsync<List<GetCampusOut>>("/campi");

        // Assert
        campi.Should().BeEmpty();
    }

    [Test]
    public async Task Should_get_many_campus()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        await client.CreateCampus("Agreste I", "Caruaru - PE");
        await client.CreateCampus("Suassuna I", "Recife - PE");

        // Act
        var campi = await client.GetAsync<List<GetCampusOut>>("/campi");

        // Assert
        campi.Should().HaveCount(2);
    }
}
