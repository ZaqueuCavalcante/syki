using Syki.Shared;
using System.Net;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Extensions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class CursoIntegrationTests : ApiTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_curso()
    {
        // Arrange
        await Login("adm@syki.com", "Adm@123");
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = new RegisterIn
        {
            Faculdade = faculdade.Id,
            Name = "Acadêmico - Nova Roma",
            Email = "academico@novaroma.com",
            Password = "Academico@123",
            Role = Academico,
        };
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var body = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" };

        // Act
        var curso = await PostAsync<CursoOut>("/cursos", body);

        // Assert
        curso.Nome.Should().Be(body.Nome);
    }

    [Test]
    public async Task Deve_criar_varios_cursos()
    {
        // Arrange
        await Login("adm@syki.com", "Adm@123");
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = new RegisterIn
        {
            Faculdade = faculdade.Id,
            Name = "Acadêmico - Nova Roma",
            Email = "academico@novaroma.com",
            Password = "Academico@123",
            Role = Academico,
        };
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        // Act
        await PostAsync("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" });
        await PostAsync("/cursos", new CursoIn { Nome = "Direito" });
        
        // Assert
        var cursos = await GetAsync<List<CursoOut>>("/cursos");
        cursos.Should().HaveCount(2);
    }

    [Test]
    public async Task Nao_deve_criar_um_novo_curso_quando_o_usuario_nao_esta_logado()
    {
        // Arrange
        var body = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" };

        // Act
        var response = await _client.PostAsync("/cursos", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_criar_um_novo_curso_quando_o_usuario_nao_tem_permissao()
    {
        // Arrange
        await Login("adm@syki.com", "Adm@123");
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = new RegisterIn
        {
            Faculdade = faculdade.Id,
            Name = "Professor - Nova Roma",
            Email = "professor@novaroma.com",
            Password = "Professor@123",
            Role = Professor,
        };
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var body = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" };

        // Act
        var response = await _client.PostAsync("/cursos", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
