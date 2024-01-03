using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Extensions;
using Syki.Back.Exceptions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class UsersIntegrationTests : IntegrationTestBase
{
    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAdm))]
    public async Task Deve_registrar_um_novo_usuario_com_role_permitida(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var body = UserIn.New(faculdade.Id, role);

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
        await RegisterAndLogin(faculdade.Id, role);

        var body = new UserIn { Name = "Zaqueu" };

        // Act
        var response = await _client.PostAsync("/users", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.InvalidRoles))]
    public async Task Nao_deve_registrar_um_novo_usuario_com_role_invalida(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var body = UserIn.New(faculdade.Id, role);

        // Act
        var response = await _client.PostAsync("/users", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0010);  
    }

    [Test]
    public async Task Nao_deve_registrar_um_novo_usuario_com_faculdade_invalida()
    {
        // Arrange
        await Login("adm@syki.com", "Adm@123");

        var body = UserIn.New(Guid.NewGuid(), Academico);

        // Act
        var response = await _client.PostAsync("/users", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0011);  
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.InvalidEmails))]
    public async Task Nao_deve_registrar_um_novo_usuario_com_email_invalido(string email)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var body = new UserIn
        {
            Faculdade = faculdade.Id,
            Name = "Acadêmico - Nova Roma",
            Email = email,
            Password = "Academico@123",
            Role = Academico,
        };

        // Act
        var response = await _client.PostAsync("/users", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0013);  
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.InvalidPasswords))]
    public async Task Nao_deve_registrar_um_novo_usuario_com_senha_fraca(string password)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var body = new UserIn
        {
            Faculdade = faculdade.Id,
            Name = "Acadêmico - Nova Roma",
            Email = "academico@novaroma.com",
            Password = password,
            Role = Academico,
        };

        // Act
        var response = await _client.PostAsync("/users", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0012);  
    }
}
