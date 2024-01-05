using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Extensions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class ProfessoresIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_professor()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var body = new ProfessorIn { Nome = "Chico" };

        // Act
        var professor = await PostAsync<ProfessorOut>("/professores", body);

        // Assert
        professor.Id.Should().NotBeEmpty();
        professor.Nome.Should().Be(body.Nome);
    }

    [Test]
    public async Task Deve_criar_varios_professores_para_uma_mesma_faculdade()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        // Act
        await PostAsync<ProfessorOut>("/professores", new ProfessorIn { Nome = "Chico" });
        await PostAsync<ProfessorOut>("/professores", new ProfessorIn { Nome = "Maju" });

        // Assert
        var professores = await GetAsync<List<ProfessorOut>>("/professores");
        professores.Should().HaveCount(2);
    }

    [Test]
    public async Task Nao_deve_criar_um_novo_professor_quando_o_usuario_esta_deslogado()
    {
        // Arrange
        var body = new ProfessorIn { Nome = "Chico" };

        // Act
        var response = await _client.PostAsync("/professores", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAcademico))]
    public async Task Nao_deve_criar_um_novo_professor_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, role);

        var body = new ProfessorIn { Nome = "Chico" };

        // Act
        var response = await _client.PostAsync("/professores", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAcademico))]
    public async Task Nao_deve_retornar_os_professores_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, role);

        // Act
        var response = await _client.GetAsync("/professores");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task Deve_retornar_apenas_os_professores_da_faculdade_do_usuario_logado()
    {
        // Arrange
        var novaRoma = await CreateFaculdade("Nova Roma");
        var userNovaRoma = UserIn.New(novaRoma.Id, Academico);
        await RegisterUser(userNovaRoma);

        var ufpe = await CreateFaculdade("UFPE");
        var userUfpe = UserIn.New(ufpe.Id, Academico);
        await RegisterUser(userUfpe);

        await Login(userNovaRoma.Email, userNovaRoma.Password);
        var bodyNovaRoma = new ProfessorIn { Nome = "Chico" };
        var professor = await PostAsync<CampusOut>("/professores", bodyNovaRoma);

        await Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new ProfessorIn { Nome = "Maju" };
        await PostAsync<CampusOut>("/professores", bodyUfpe);

        // Act
        await Login(userNovaRoma.Email, userNovaRoma.Password);

        // Assert
        var campi = await GetAsync<List<ProfessorOut>>("/professores");
        campi.Should().HaveCount(1);
        campi[0].Id.Should().Be(professor.Id);
        campi[0].Nome.Should().Be(bodyNovaRoma.Nome);
    }
}
