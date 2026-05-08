using Syki.Tests.Integration.Base;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Validation
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    [Test]
    [TestCase("")]
    [TestCase("invalid@")]
    [TestCase("invalidemail")]
    [TestCase("@invalid.com")]
    [TestCase("invalid@.com")]
    [TestCase("invalid email@test.com")]
    public async Task Users_RegisterUser_Should_not_create_user_with_invalid_email(string email)
    {
        // Arrange
        var client = _api.GetTestsClient();

        // Act
        var response = await client.RegisterUser(email);

        // Assert
        response.ShouldBeError(InvalidEmail.I);
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Business logic errors
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    #region Business logic errors

    [Test]
    public async Task Users_RegisterUser_Should_not_create_user_with_already_used_email()
    {
        // Arrange
        var client = _api.GetTestsClient();

        var email = DataGen.Email;
        await client.RegisterUser(email);

        // Act
        var response = await client.RegisterUser(email);

        // Assert
        response.ShouldBeError(EmailAlreadyUsed.I);
    }

    [Test]
    public async Task Users_RegisterUser_Should_not_create_user_with_already_used_email_different_casing()
    {
        // Arrange
        var client = _api.GetTestsClient();

        var email = DataGen.Email.ToUpper();
        await client.RegisterUser(email);

        // Act
        var response = await client.RegisterUser(email.ToLower());

        // Assert
        response.ShouldBeError(EmailAlreadyUsed.I);
    }

    #endregion

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Happy path
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    #region Happy path

    [Test]
    public async Task Users_RegisterUser_Should_create_a_new_user()
    {
       // Arrange
        var client = _api.GetTestsClient();
        var email = DataGen.Email;

        // Act
        var response = await client.RegisterUser(email);

        // Assert
        response.ShouldBeSuccess();
    }

    #endregion
}
