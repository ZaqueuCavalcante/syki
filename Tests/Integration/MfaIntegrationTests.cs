using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;

namespace Syki.Tests.Integration;

[TestFixture]
public class MfaIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Nao_deve_retornar_a_mfa_key_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/users/mfa-key");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllUsersRoles))]
    public async Task Deve_retornar_a_mfa_key_independente_da_role_do_usuario(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, role);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        // Act
        var response = await GetAsync<MfaKeyOut>("/users/mfa-key");

        // Assert
        response.Key.Should().HaveLength(32);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllUsersRoles))]
    public async Task Deve_realizar_o_setup_da_mfa_independente_da_role_do_usuario(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, role);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var keyResponse = await GetAsync<MfaKeyOut>("/users/mfa-key");
        var token = keyResponse.Key.ToMfaToken();

        // Act
        var response = await PostAsync<MfaSetupOut>("/users/mfa-setup", new MfaSetupIn { Token = token });

        // Assert
        response.Ok.Should().BeTrue();
    }
}
