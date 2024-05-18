namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_return_all_institutions()
    {
        // Arrange
        var client = _factory.GetClient();
        await client.RegisterUser(_factory);

        var admClient = await _factory.LoggedAsAdm();

        // Act
        var institutions = await admClient.GetInstitutions();

        // Assert
        institutions.Should().HaveCountGreaterThanOrEqualTo(1);
    }
}
