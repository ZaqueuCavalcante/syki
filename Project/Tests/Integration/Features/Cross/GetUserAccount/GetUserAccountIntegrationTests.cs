namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_get_exato_adm_user_account()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        // Act
        var account = await client.Cross.GetUserAccount();

        // Assert
        account.Id.Should().NotBeEmpty();
        account.Role.Should().Be("OfficeAdm");
        account.Name.Should().Be(ExatoAdmName);
        account.Email.Should().Be(ExatoAdmEmail);
        account.Organization.Should().Be("Exato Digital");
    }
}
