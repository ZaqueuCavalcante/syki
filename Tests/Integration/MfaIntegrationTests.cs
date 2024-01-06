using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class MfaIntegrationTests : IntegrationTestBase
{
    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllUsersRoles))]
    public async Task Deve_retornar_a_mfa_key_independente_da_role_do_usuario(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, role);

        // Act
        var response = await GetAsync<MfaKeyOut>("/users/mfa-key");

        // Assert
        response.Key.Should().HaveLength(32);
    }

    [Test]
    public async Task Nao_deve_realizar_o_setup_da_mfa_quando_informar_codigo_errado()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, Academico);
        var token = Guid.NewGuid().ToHashCode().ToString()[..6];

        // Act
        var response = await PostAsync<MfaSetupOut>("/users/mfa-setup", new MfaSetupIn { Token = token });

        // Assert
        response.Ok.Should().BeFalse();
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllUsersRoles))]
    public async Task Deve_realizar_o_setup_da_mfa_independente_da_role_do_usuario(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, role);

        var keyResponse = await GetAsync<MfaKeyOut>("/users/mfa-key");
        var token = keyResponse.Key.ToMfaToken();

        // Act
        var response = await PostAsync<MfaSetupOut>("/users/mfa-setup", new MfaSetupIn { Token = token });

        // Assert
        response.Ok.Should().BeTrue();
    }
}
