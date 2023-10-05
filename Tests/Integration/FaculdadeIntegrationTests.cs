using Syki.Dtos;
using System.Net;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Extensions;

namespace Syki.Tests.Integration;

[TestFixture]
public class FauldadeIntegrationTests : ApiTestBase
{
    [Test]
    public async Task Deve_criar_uma_nova_faculdade()
    {
        // Arrange
        await Login("adm@syki.com");

        var body = new FaculdadeIn { Nome = "Nova Roma" };

        // Act
        var faculdade = await PostAsync<FaculdadeOut>("/faculdades", body);
        
        // Assert
        faculdade.Nome.Should().Be(body.Nome);
    }

    [Test]
    public async Task Deve_criar_varias_faculdades()
    {
        // Arrange
        await Login("adm@syki.com");

        // Act
        await PostAsync("/faculdades", new FaculdadeIn { Nome = "Nova Roma" });
        await PostAsync("/faculdades", new FaculdadeIn { Nome = "UFPE" });

        // Assert
        var faculdades = await GetAsync<List<FaculdadeOut>>("/faculdades");
        faculdades.Should().HaveCount(2);
    }

    [Test]
    public async Task Nao_deve_criar_uma_nova_faculdade_quando_o_usuario_nao_esta_logado()
    {
        // Arrange
        var body = new FaculdadeIn { Nome = "Nova Roma" };

        // Act
        var response = await _client.PostAsync("/faculdades", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_criar_uma_nova_faculdade_quando_o_usuario_nao_tem_permissao()
    {
        // Arrange
        await Login("academico@novaroma.com");

        var body = new FaculdadeIn { Nome = "Nova Roma" };

        // Act
        var response = await _client.PostAsync("/faculdades", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
