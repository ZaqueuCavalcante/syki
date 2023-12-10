using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Extensions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class FauldadeIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_uma_nova_faculdade()
    {
        // Arrange
        await Login("adm@syki.com", "Adm@123");

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
        await Login("adm@syki.com", "Adm@123");

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
        await Login("adm@syki.com", "Adm@123");

        var faculdade = await CreateFaculdade("Nova Roma");

        var user = new UserIn
        {
            Faculdade = faculdade.Id,
            Name = "Acadêmico - Nova Roma",
            Email = "academico@novaroma.com",
            Password = "Academico@123",
            Role = Academico,
        };
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var body = new FaculdadeIn { Nome = "UFPE" };

        // Act
        var response = await _client.PostAsync("/faculdades", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
