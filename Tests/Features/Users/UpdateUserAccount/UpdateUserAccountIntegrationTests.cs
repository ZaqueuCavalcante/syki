namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Users_UpdateUserAccount_Should_return_401_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.UpdateUserAccount();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Validation errors

    [Test]
    [TestCase("")]
    public async Task Users_UpdateUserAccount_Should_not_update_account_with_invalid_name(string name)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateUserAccount(name);

        // Assert
        result.ShouldBeError(InvalidUserName.I);
    }

    [Test]
    public async Task Users_UpdateUserAccount_Should_not_update_account_with_name_longer_than_100_chars()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateUserAccount(new string('a', 101));

        // Assert
        result.ShouldBeError(InvalidUserName.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Users_UpdateUserAccount_Should_update_account_name()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateUserAccount("Edson Gomes");

        // Assert
        result.ShouldBeSuccess();

        var account = await client.GetUserAccount().Success();
        account.Id.Should().Be(client.User.Id);
        account.Name.Should().Be("Edson Gomes");
    }

    [Test]
    public async Task Users_UpdateUserAccount_Should_update_account_name_for_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.UpdateUserAccount("Maria Júlia");

        // Assert
        result.ShouldBeSuccess();

        var account = await client.GetUserAccount().Success();
        account.Id.Should().Be(client.User.Id);
        account.Name.Should().Be("Maria Júlia");
    }

    #endregion
}
