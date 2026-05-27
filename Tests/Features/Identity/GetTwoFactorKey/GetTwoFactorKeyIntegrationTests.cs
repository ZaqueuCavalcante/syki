namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    #region Authentication

    [Test]
    public async Task Identity_GetTwoFactorKey_Should_not_get_2fa_key_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var response = await client.GetTwoFactorKey();

        // Assert
        response.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task GetTwoFactorKey_Should_get_2fa_key()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.GetTwoFactorKey();

        // Assert
        response.Success.Key.Should().HaveLength(32);
        response.Success.TwoFactorEnabled.Should().BeFalse();
        response.Success.QrCodeBase64.Should().StartWith("data:image/png;base64,iVBORw0KGgoAAAAN");
    }

    [Test]
    public async Task GetTwoFactorKey_Should_get_same_2fa_key_for_many_gets()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response00 = await client.GetTwoFactorKey();
        var response01 = await client.GetTwoFactorKey();
        var response02 = await client.GetTwoFactorKey();

        // Assert
        response00.Success.Key.Should().HaveLength(32);
        response00.Success.Key.Should().Be(response01.Success.Key);
        response00.Success.Key.Should().Be(response02.Success.Key);
    }

    #endregion
}
