namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_create_a_new_campus()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");

        // Assert
        campus.Id.Should().NotBeEmpty();
        campus.Name.Should().Be("Agreste I");
        campus.City.Should().Be("Caruaru - PE");
    }
}
