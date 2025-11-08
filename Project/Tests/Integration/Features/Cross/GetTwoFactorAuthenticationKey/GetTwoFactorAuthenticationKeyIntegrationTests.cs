namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_get_2fa_key()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        // Act
        var response = await client.Cross.GetTwoFactorAuthenticationKey();

        // Assert
        response.Key.Should().HaveLength(32);
        response.TwoFactorEnabled.Should().BeFalse();
        response.QrCodeBase64.Should().StartWith("data:image/png;base64,iVBORw0KGgoAAAAN");
    }

    [Test]
    public async Task Should_get_same_2fa_key_for_many_gets()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        // Act
        var response00 = await client.Cross.GetTwoFactorAuthenticationKey();
        var response01 = await client.Cross.GetTwoFactorAuthenticationKey();
        var response02 = await client.Cross.GetTwoFactorAuthenticationKey();

        // Assert
        response00.Key.Should().HaveLength(32);
        response00.Key.Should().Be(response01.Key);
        response00.Key.Should().Be(response02.Key);
    }
}
