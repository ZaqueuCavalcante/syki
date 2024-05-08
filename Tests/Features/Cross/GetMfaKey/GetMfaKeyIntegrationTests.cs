namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_get_mfa_key()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        var response = await client.GetMfaKey();

        // Assert
        response.Key.Should().HaveLength(32);
    }

    [Test]
    public async Task Should_get_same_mfa_key_for_many_gets()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        var response00 = await client.GetMfaKey();
        var response01 = await client.GetMfaKey();
        var response02 = await client.GetMfaKey();

        // Assert
        response00.Key.Should().HaveLength(32);
        response00.Key.Should().Be(response01.Key);
        response00.Key.Should().Be(response02.Key);
    }
}
