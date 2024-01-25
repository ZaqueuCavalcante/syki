using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.AllUsersRoles))]
    public async Task Deve_retornar_a_mfa_key_independente_da_role_do_usuario(string role)
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, role);

        // Act
        var response = await client.GetAsync<MfaKeyOut>("/users/mfa-key");

        // Assert
        response.Key.Should().HaveLength(32);
    }

    [Test]
    public async Task Nao_deve_realizar_o_setup_da_mfa_quando_informar_codigo_errado()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);
        var token = Guid.NewGuid().ToHashCode().ToString()[..6];

        // Act
        var response = await client.PostAsync<MfaSetupOut>("/users/mfa-setup", new MfaSetupIn { Token = token });

        // Assert
        response.Ok.Should().BeFalse();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAdm))]
    public async Task Deve_realizar_o_setup_da_mfa_independente_da_role_do_usuario(string role)
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, role);

        var keyResponse = await client.GetAsync<MfaKeyOut>("/users/mfa-key");
        var token = keyResponse.Key.ToMfaToken();

        // Act
        var response = await client.PostAsync<MfaSetupOut>("/users/mfa-setup", new MfaSetupIn { Token = token });

        // Assert
        response.Ok.Should().BeTrue();
    }
}
