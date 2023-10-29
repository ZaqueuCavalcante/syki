using Syki.Shared;
using System.Net;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Extensions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class DisciplinaIntegrationTests : ApiTestBase
{
    [Test]
    public async Task Deve_criar_uma_nova_disciplina()
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

        var body = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };

        // Act
        var disciplina = await PostAsync<DisciplinaOut>("/disciplinas", body);

        // Assert
        disciplina.Nome.Should().Be(body.Nome);
        disciplina.CargaHoraria.Should().Be(body.CargaHoraria);
    }

    [Test]
    public async Task Deve_criar_varias_disciplinas()
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

        var body = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };

        // Act
        var response = await _client.PostAsync("/disciplinas", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
