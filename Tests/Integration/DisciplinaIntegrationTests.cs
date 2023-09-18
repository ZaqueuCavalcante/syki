using Syki.Dtos;
using System.Net;
using Syki.Tests.Base;
using Syki.Extensions;
using NUnit.Framework;
using FluentAssertions;

namespace Syki.Tests.Integration;

[TestFixture]
public class DisciplinaIntegrationTests : ApiTestBase
{
    [Test]
    public async Task Deve_criar_uma_nova_disciplina()
    {
        // Arrange
        await CreateFaculdade("Nova Roma");

        await Login("academico@novaroma.com");

        var body = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };

        // Act
        var disciplina = await PostAsync<DisciplinaOut>("/disciplinas", body);

        // Assert
        disciplina.Id.Should().Be(1);
        disciplina.Nome.Should().Be(body.Nome);
        disciplina.CargaHoraria.Should().Be(body.CargaHoraria);
    }

    [Test]
    public async Task Deve_criar_varias_disciplinas()
    {
        // Arrange
        await CreateFaculdade("Nova Roma");

        await Login("academico@novaroma.com");

        // Act
        await PostAsync("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 });
        await PostAsync("/disciplinas", new DisciplinaIn { Nome = "Estrutura de Dados", CargaHoraria = 72 });

        // Assert
        var disciplinas = await GetAsync<List<DisciplinaOut>>("/disciplinas");
        disciplinas.Should().HaveCount(2);
    }

    [Test]
    public async Task Nao_deve_criar_uma_nova_disciplina_quando_o_usuario_nao_esta_logado()
    {
        // Arrange
        var body = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };

        // Act
        var response = await _client.PostAsync("/disciplinas", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_criar_uma_nova_disciplina_quando_o_usuario_nao_tem_permissao()
    {
        // Arrange
        await Login("professor@novaroma.com");

        var body = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };

        // Act
        var response = await _client.PostAsync("/disciplinas", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
