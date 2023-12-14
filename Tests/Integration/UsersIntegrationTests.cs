using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Extensions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class UsersIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_registrar_um_novo_usuario_com_role_de_academico()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var body = new UserIn
        {
            Faculdade = faculdade.Id,
            Name = "Zaqueu",
            Email = "zaqueu@syki.com",
            Password = "Zaqueu@123",
            Role = Academico,
        };

        // Act
        var user = await PostAsync<UserOut>("/users", body);

        // Assert
        user.Id.Should().NotBeEmpty();
        user.Nome.Should().Be(body.Name);
        user.Email.Should().Be(body.Email);
        user.FaculdadeId.Should().Be(body.Faculdade);
    }

    [Test]
    public async Task Nao_deve_registrar_um_novo_usuario_quando_o_usuario_esta_deslogado()
    {
        // Arrange
        var body = new UserIn { Name = "Zaqueu" };

        // Act
        var response = await _client.PostAsync("/users", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAdm))]
    public async Task Nao_deve_registrar_um_novo_usuario_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, role);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var body = new UserIn { Name = "Zaqueu" };

        // Act
        var response = await _client.PostAsync("/users", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
