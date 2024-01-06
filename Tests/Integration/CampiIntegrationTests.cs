using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Extensions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class CampiIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_campus()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var body = new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" };

        // Act
        var campus = await PostAsync<CampusOut>("/campi", body);

        // Assert
        campus.Id.Should().NotBeEmpty();
        campus.Nome.Should().Be(body.Nome);
        campus.Cidade.Should().Be(body.Cidade);
    }

    [Test]
    public async Task Deve_criar_varios_campus_para_uma_mesma_faculdade()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        // Act
        await PostAsync("/campi", new CampusIn { Nome = "Suassuna", Cidade = "Recife - PE" });
        await PostAsync("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });

        // Assert
        var campi = await GetAsync<List<CampusOut>>("/campi");
        campi.Should().HaveCount(2);
    }

    [Test]
    public async Task Deve_atualizar_um_campus()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var body = new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" };
        var campus = await PostAsync<CampusOut>("/campi", body);

        // Act
        campus.Nome = "Agreste II";
        campus.Cidade = "Bonito - PE";
        var updatedCampus = await PutAsync<CampusOut>("/campi", campus);

        // Assert
        updatedCampus.Id.Should().Be(campus.Id);
        updatedCampus.Nome.Should().Be(campus.Nome);
        updatedCampus.Cidade.Should().Be(campus.Cidade);
    }

    [Test]
    public async Task Deve_retornar_apenas_os_campus_da_faculdade_do_usuario_logado()
    {
        // Arrange
        var novaRoma = await CreateFaculdade("Nova Roma");
        var userNovaRoma = UserIn.New(novaRoma.Id, Academico);
        await RegisterUser(userNovaRoma);

        var ufpe = await CreateFaculdade("UFPE");
        var userUfpe = UserIn.New(ufpe.Id, Academico);
        await RegisterUser(userUfpe);

        await Login(userNovaRoma.Email, userNovaRoma.Password);
        var bodyNovaRoma = new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" };
        await PostAsync<CampusOut>("/campi", bodyNovaRoma);

        await Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new CampusIn { Nome = "Suassuna", Cidade = "Recife - PE" };
        await PostAsync<CampusOut>("/campi", bodyUfpe);

        // Act
        await Login(userNovaRoma.Email, userNovaRoma.Password);

        var campi = await GetAsync<List<CampusOut>>("/campi");
        campi.Should().HaveCount(1);

        // Assert
        campi[0].Id.Should().NotBeEmpty();
        campi[0].Nome.Should().Be(bodyNovaRoma.Nome);
        campi[0].Cidade.Should().Be(bodyNovaRoma.Cidade);
    }
}
