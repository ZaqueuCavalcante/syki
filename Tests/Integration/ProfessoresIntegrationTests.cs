using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_professor()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await client.RegisterUser(user);
        await client.Login(user.Email, user.Password);

        var body = new ProfessorIn { Nome = "Chico" };

        // Act
        var professor = await client.PostAsync<ProfessorOut>("/professores", body);

        // Assert
        professor.Id.Should().NotBeEmpty();
        professor.Nome.Should().Be(body.Nome);
    }

    [Test]
    public async Task Deve_criar_varios_professores_para_uma_mesma_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await client.RegisterUser(user);
        await client.Login(user.Email, user.Password);

        // Act
        await client.PostAsync<ProfessorOut>("/professores", new ProfessorIn { Nome = "Chico" });
        await client.PostAsync<ProfessorOut>("/professores", new ProfessorIn { Nome = "Maju" });

        // Assert
        var professores = await client.GetAsync<List<ProfessorOut>>("/professores");
        professores.Should().HaveCount(2);
    }

    [Test]
    public async Task Deve_retornar_apenas_os_professores_da_faculdade_do_usuario_logado()
    {
        // Arrange
        var client = _factory.CreateClient();
        var novaRoma = await client.CreateFaculdade("Nova Roma");
        var userNovaRoma = UserIn.New(novaRoma.Id, Academico);
        await client.RegisterUser(userNovaRoma);

        var ufpe = await client.CreateFaculdade("UFPE");
        var userUfpe = UserIn.New(ufpe.Id, Academico);
        await client.RegisterUser(userUfpe);

        await client.Login(userNovaRoma.Email, userNovaRoma.Password);
        var bodyNovaRoma = new ProfessorIn { Nome = "Chico" };
        var professor = await client.PostAsync<CampusOut>("/professores", bodyNovaRoma);

        await client.Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new ProfessorIn { Nome = "Maju" };
        await client.PostAsync<CampusOut>("/professores", bodyUfpe);

        // Act
        await client.Login(userNovaRoma.Email, userNovaRoma.Password);

        // Assert
        var campi = await client.GetAsync<List<ProfessorOut>>("/professores");
        campi.Should().HaveCount(1);
        campi[0].Id.Should().Be(professor.Id);
        campi[0].Nome.Should().Be(bodyNovaRoma.Nome);
    }
}
