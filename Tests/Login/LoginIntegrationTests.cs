using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_not_login_random_user()
    {
        // Arrange
        var client = _factory.CreateClient();
        var data = new LoginIn { Email = "academico@novaroma.com", Password = "Academico@123" };

        // Act
        var result = await client.PostAsync<LoginOut>("/login", data);

        // Assert
        result.WrongEmailOrPassword.Should().BeTrue();
    }

    [Test]
    public async Task Should_not_login_user_with_wrong_email()
    {
        // Arrange
        var client = _factory.CreateClient();
        var institution = await client.CreateInstitution();
        var user = CreateUserIn.New(institution.Id, Academico);
        await client.RegisterUser(user);

        var data = new LoginIn { Email = user.Email + "1", Password = user.Password };

        // Act
        var result = await client.PostAsync<LoginOut>("/login", data);

        // Assert
        result.WrongEmailOrPassword.Should().BeTrue();
    }

    [Test]
    public async Task Should_not_login_user_with_wrong_password()
    {
        // Arrange
        var client = _factory.CreateClient();
        var institution = await client.CreateInstitution();
        var user = CreateUserIn.New(institution.Id, Academico);
        await client.RegisterUser(user);

        var data = new LoginIn { Email = user.Email, Password = user.Password + "1" };

        // Act
        var result = await client.PostAsync<LoginOut>("/login", data);

        // Assert
        result.WrongEmailOrPassword.Should().BeTrue();
    }

    [Test]
    public async Task Should_login_user_without_mfa()
    {
        // Arrange
        var client = _factory.CreateClient();
        var institution = await client.CreateInstitution();
        var user = CreateUserIn.New(institution.Id, Academico);
        await client.RegisterUser(user);

        var data = new LoginIn { Email = user.Email, Password = user.Password };

        // Act
        var response = await client.PostAsync<LoginOut>("/login", data);

        // Assert
        response.AccessToken.Should().StartWith("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.");
    }

    [Test]
    public async Task Should_not_login_user_with_correct_email_and_password_but_needs_mfa()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution();
        var user = await client.RegisterAndLogin(faculdade.Id, Academico);

        var keyResponse = await client.GetAsync<GetMfaKeyOut>("/mfa/key");
        var token = keyResponse.Key.ToMfaToken();
        await client.PostAsync<SetupMfaOut>("/mfa/setup", new SetupMfaIn { Token = token });

        client.RemoveAuthToken();

        var data = new LoginIn { Email = user.Email, Password = user.Password };

        // Act
        var result = await client.PostAsync<LoginOut>("/login", data);

        // Assert
        result.RequiresTwoFactor.Should().BeTrue();
    }
}
