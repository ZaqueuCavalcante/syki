using System.Net;
using Syki.Domain;
using Syki.Tests.Base;
using Syki.Extensions;
using NUnit.Framework;
using FluentAssertions;

namespace Syki.Tests.Integration;

[TestFixture]
public class AlunoIntegrationTests : ApiTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_aluno()
    {
        // Arrange
        var faculdadeIn = new FaculdadeIn { Nome = "Nova Roma" };
        var faculdadeResponse = await _client.PostAsync("/faculdades", faculdadeIn.ToStringContent());
        var faculdade = await faculdadeResponse.DeserializeTo<FaculdadeOut>();
        
        var body = new AlunoIn { Nome = "Zaqueu" };

        // Act
        var response = await _client.PostAsync("/alunos", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var project = await response.DeserializeTo<AlunoOut>();
        project.Id.Should().Be(1);
        project.Nome.Should().Be(body.Nome);
        project.Matricula.Should().StartWith($"{DateTime.Now.Year}").And.HaveLength(12);
    }
}
