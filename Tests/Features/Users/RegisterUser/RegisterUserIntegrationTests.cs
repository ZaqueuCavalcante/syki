namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    #region Validation errors

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
        var client = _back.GetTestsClient();

        // Act
        var response = await client.RegisterUser(email);

        // Assert
        response.ShouldBeError(InvalidEmail.I);
    }

    [Test]
    public async Task Users_RegisterUser_Should_not_create_user_with_already_used_email()
    {
        // Arrange
        var client = _back.GetTestsClient();

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
        var client = _back.GetTestsClient();

        var email = DataGen.Email.ToUpper();
        await client.RegisterUser(email);

        // Act
        var response = await client.RegisterUser(email.ToLower());

        // Assert
        response.ShouldBeError(EmailAlreadyUsed.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Users_RegisterUser_Should_create_a_new_user()
    {
       // Arrange
        var client = _back.GetTestsClient();
        var email = DataGen.Email;

        // Act
        var response = await client.RegisterUser(email);

        // Assert
        response.ShouldBeSuccess();
    }

    #endregion
}
