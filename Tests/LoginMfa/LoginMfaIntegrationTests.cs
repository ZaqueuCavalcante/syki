using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    // [Test]
    public async Task Should_not_login_when_try_get_jwt_whith_right_mfa_code_but_without_supply_email_and_password()
    {
        // Arrange
        var client = _factory.CreateClient();
        var institution = await client.CreateInstitution();
        await client.RegisterAndLogin(institution.Id, Academico);

        var keyResponse = await client.GetAsync<GetMfaKeyOut>("/mfa/key");
        var token = keyResponse.Key.ToMfaToken();
        await client.PostAsync<SetupMfaOut>("/mfa/setup", new SetupMfaIn { Token = token });

        client.RemoveAuthToken();

        var body = new LoginMfaIn { Code = token };

        // Act
        var response = await client.PostAsync<LoginMfaOut>("/mfa/login", body);

        // Assert
        response.AccessToken.Should().BeNull();
        response.Wrong2FactorCode.Should().BeTrue();
    }

    // [Test]
    public async Task Should_not_login_when_supply_wrong_mfa_code()
    {
        // Arrange
        var client = _factory.CreateClient();
        var institution = await client.CreateInstitution();
        var user = await client.RegisterAndLogin(institution.Id, Academico);

        var keyResponse = await client.GetAsync<GetMfaKeyOut>("/mfa/key");
        var token = keyResponse.Key.ToMfaToken();
        await client.PostAsync<SetupMfaOut>("/mfa/setup", new SetupMfaIn { Token = token });

        client.RemoveAuthToken();

        var data = new LoginIn { Email = user.Email, Password = user.Password };
        await client.PostAsync("/login", data);

        var body = new LoginMfaIn { Code = Guid.NewGuid().ToHashCode().ToString()[..6] };

        // Act
        var response = await client.PostAsync<LoginMfaOut>("/mfa/login", body);

        // Assert
        response.AccessToken.Should().BeNull();
        response.Wrong2FactorCode.Should().BeTrue();
    }

    // [Test]
    public async Task Should_login_when_supply_right_mfa_code()
    {
        // Arrange
        var client = _factory.CreateClient();
        var institution = await client.CreateInstitution();
        var user = await client.RegisterAndLogin(institution.Id, Academico);

        var keyResponse = await client.GetAsync<GetMfaKeyOut>("/mfa/key");
        var token = keyResponse.Key.ToMfaToken();
        await client.PostAsync<SetupMfaOut>("/mfa/setup", new SetupMfaIn { Token = token });

        client.RemoveAuthToken();

        var data = new LoginIn { Email = user.Email, Password = user.Password };
        await client.PostHttpAsync("/login", data);

        var body = new LoginMfaIn { Code = token };

        // Act
        var response = await client.PostAsync<LoginMfaOut>("/mfa/login", body);

        // Assert
        response.AccessToken.Should().StartWith("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.");
    }
}
