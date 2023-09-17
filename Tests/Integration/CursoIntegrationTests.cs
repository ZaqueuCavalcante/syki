using Syki.Dtos;
using System.Net;
using Syki.Tests.Base;
using Syki.Extensions;
using NUnit.Framework;
using FluentAssertions;

namespace Syki.Tests.Integration;

[TestFixture]
public class CursoIntegrationTests : ApiTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_curso()
    {
        // Arrange
        await CreateFaculdade("Nova Roma");

        await Login("academico@novaroma.com");

        var body = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" };

        // Act
        var curso = await PostAsync<CursoOut>("/cursos", body);

        // Assert
        curso.Id.Should().Be(1);
        curso.Nome.Should().Be(body.Nome);
    }

    [Test]
    public async Task Deve_criar_varios_cursos()
    {
        // Arrange
        await CreateFaculdade("Nova Roma");

        await Login("academico@novaroma.com");

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
        await Login("professor@novaroma.com");

        var body = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" };

        // Act
        var response = await _client.PostAsync("/cursos", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
