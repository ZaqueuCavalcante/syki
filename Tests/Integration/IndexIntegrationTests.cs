using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;

namespace Syki.Tests.Integration;

[TestFixture]
public class IndexIntegrationTests : IntegrationTestBase
{
    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAdm))]
    public async Task Nao_deve_retornar_os_dados_de_index_quando_o_usuario_nao_eh_adm(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, role);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        // Act
        var response = await _client.GetAsync("/index/adm");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAcademico))]
    public async Task Nao_deve_retornar_os_dados_de_index_quando_o_usuario_nao_eh_academico(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, role);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        // Act
        var response = await _client.GetAsync("/index/academico");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAluno))]
    public async Task Nao_deve_retornar_os_dados_de_index_quando_o_usuario_nao_eh_aluno(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, role);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        // Act
        var response = await _client.GetAsync("/index/aluno");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
