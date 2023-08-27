using System.Net;
using Syki.Domain;
using Syki.Tests.Base;
using Syki.Extensions;
using NUnit.Framework;
using FluentAssertions;

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
        var response = await _client.PostAsync("/faculdades", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var faculdade = await response.DeserializeTo<FaculdadeOut>();
        faculdade.Id.Should().Be(1);
        faculdade.Nome.Should().Be(body.Nome);
    }

    [Test]
    public async Task Deve_criar_varias_faculdades()
    {
        // Arrange
        await Login("adm@syki.com");

        // Act
        await _client.PostAsync("/faculdades", new FaculdadeIn { Nome = "Nova Roma" }.ToStringContent());
        await _client.PostAsync("/faculdades", new FaculdadeIn { Nome = "UFPE" }.ToStringContent());
        
        // Assert
        var response = await _client.GetAsync("/faculdades");
        var faculdades = await response.DeserializeTo<List<FaculdadeOut>>();
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
