using Syki.Dtos;
using System.Net;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Extensions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class CampusIntegrationTests : ApiTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_campus()
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

        var body = new CampusIn { Nome = "Campus Caruaru" };

        // Act
        var campus = await PostAsync<CampusOut>("/campi", body);
        
        // Assert
        campus.Nome.Should().Be(body.Nome);
    }

    [Test]
    public async Task Deve_criar_varios_campus_para_uma_mesma_faculdade()
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
        await PostAsync("/campi", new CampusIn { Nome = "Campus Recife" });
        await PostAsync("/campi", new CampusIn { Nome = "Campus Caruaru" });
        
        // Assert
        var campi = await GetAsync<List<CampusOut>>("/campi");
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

        var body = new CampusIn { Nome = "Campus Caruaru" };

        // Act
        var response = await _client.PostAsync("/campi", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
