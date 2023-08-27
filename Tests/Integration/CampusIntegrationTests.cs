using System.Net;
using Syki.Domain;
using Syki.Tests.Base;
using Syki.Extensions;
using NUnit.Framework;
using FluentAssertions;

namespace Syki.Tests.Integration;

[TestFixture]
public class CampusIntegrationTests : ApiTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_campus()
    {
        // Arrange
        await CreateFaculdade("Nova Roma");

        await Login("academico@novaroma.com");

        var body = new CampusIn { Nome = "Campus Caruaru" };

        // Act
        var response = await _client.PostAsync("/campi", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var campus = await response.DeserializeTo<CampusOut>();
        campus.Id.Should().Be(1);
        campus.FaculdadeId.Should().Be(1);
        campus.Nome.Should().Be(body.Nome);
    }

    [Test]
    public async Task Deve_criar_varios_campus_para_uma_mesma_faculdade()
    {
        // Arrange
        await CreateFaculdade("Nova Roma");

        await Login("academico@novaroma.com");

        // Act
        await _client.PostAsync("/campi", new CampusIn { Nome = "Campus Recife" }.ToStringContent());
        await _client.PostAsync("/campi", new CampusIn { Nome = "Campus Caruaru" }.ToStringContent());
        
        // Assert
        var response = await _client.GetAsync("/campi");
        var campi = await response.DeserializeTo<List<CampusOut>>();
        campi.Should().HaveCount(2);
    }

    [Test]
    public async Task Nao_deve_criar_um_novo_campus_quando_o_usuario_nao_esta_logado()
    {
        // Arrange
        var body = new CampusIn { Nome = "Campus Caruaru" };

        // Act
        var response = await _client.PostAsync("/campi", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_criar_um_novo_campus_quando_o_usuario_nao_tem_permissao()
    {
        // Arrange
        await Login("professor@novaroma.com");

        var body = new CampusIn { Nome = "Campus Caruaru" };

        // Act
        var response = await _client.PostAsync("/campi", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
