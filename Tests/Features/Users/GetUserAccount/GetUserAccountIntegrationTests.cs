namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Users_GetUserAccount_Should_return_401_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetUserAccount();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Users_GetUserAccount_Should_return_account_for_director()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetUserAccount();

        // Assert
        var account = result.Success;
        account.Id.Should().Be(client.User.Id);
        account.Email.Should().Be(client.User.Email);
        account.Name.Should().Be("Seu Nome");
        account.Role.Should().Be("Diretor");
        account.UserType.Should().Be(UserType.Manager);
        account.Institution.Should().NotBeNullOrEmpty();
        account.Permissions.Should().NotBeEmpty();
    }

    [Test]
    public async Task Users_GetUserAccount_Should_return_account_for_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetUserAccount();

        // Assert
        var account = result.Success;
        account.Id.Should().Be(client.User.Id);
        account.Email.Should().Be(client.User.Email);
        account.Role.Should().Be("Professor");
        account.UserType.Should().Be(UserType.Teacher);
        account.Institution.Should().NotBeNullOrEmpty();
        account.Permissions.Should().BeEmpty();
    }

    #endregion
}
