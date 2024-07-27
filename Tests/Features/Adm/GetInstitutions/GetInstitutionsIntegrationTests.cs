namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_all_institutions()
    {
        // Arrange
        await _back.LoggedAsAcademic();

        var client = await _back.LoggedAsAdm();

        // Act
        var institutions = await client.GetInstitutions();

        // Assert
        institutions.Should().HaveCountGreaterThanOrEqualTo(1);
    }
}
