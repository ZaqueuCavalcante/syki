namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_mfa_key()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var response = await client.Http.GetMfaKey();

        // Assert
        response.Key.Should().HaveLength(32);
        response.QrCodeBase64.Should().StartWith("data:image/png;base64,iVBORw0KGgoAAAAN");
    }

    [Test]
    public async Task Should_get_same_mfa_key_for_many_gets()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var response00 = await client.Http.GetMfaKey();
        var response01 = await client.Http.GetMfaKey();
        var response02 = await client.Http.GetMfaKey();

        // Assert
        response00.Key.Should().HaveLength(32);
        response00.Key.Should().Be(response01.Key);
        response00.Key.Should().Be(response02.Key);
    }
}
