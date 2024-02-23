using Syki.Shared;
using Syki.Shared.SetupMfa;
using Syki.Shared.GetMfaKey;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidMfaTokens))]
    public async Task Should_not_setup_mfa_when_token_is_wrong(string token)
    {
        // Arrange
        var client = _factory.CreateClient();
        var institution = await client.CreateInstitution();
        await client.RegisterAndLogin(institution.Id, Academico);

        // Act
        var response = await client.PostAsync<SetupMfaOut>("/mfa/setup", new SetupMfaIn { Token = token });

        // Assert
        response.Ok.Should().BeFalse();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAdm))]
    public async Task Should_setup_mfa_for_all_user_roles(string role)
    {
        // Arrange
        var client = _factory.CreateClient();
        var institution = await client.CreateInstitution();
        await client.RegisterAndLogin(institution.Id, role);

        var keyResponse = await client.GetAsync<GetMfaKeyOut>("/mfa/key");
        var token = keyResponse.Key.ToMfaToken();

        // Act
        var response = await client.PostHttpAsync("/mfa/setup", new SetupMfaIn { Token = token });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
