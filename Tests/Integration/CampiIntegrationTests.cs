using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_campus()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        var campus = await client.NewCampus("Agreste I", "Caruaru - PE");

        // Assert
        campus.Id.Should().NotBeEmpty();
        campus.Nome.Should().Be("Agreste I");
        campus.Cidade.Should().Be("Caruaru - PE");
    }

    [Test]
    public async Task Deve_criar_varios_campus_para_uma_mesma_faculdade()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        await client.NewCampus("Suassuna", "Recife - PE");
        await client.NewCampus("Agreste I", "Caruaru - PE");

        // Assert
        var campi = await client.GetAsync<List<CampusOut>>("/campi");
        campi.Should().HaveCount(2);
    }

    [Test]
    public async Task Deve_atualizar_um_campus()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        var campus = await client.NewCampus("Agreste I", "Caruaru - PE");
        var body = new CampusUp { Id = campus.Id, Nome = "Agreste II", Cidade = "Bonito - PE" };

        // Act
        var updatedCampus = await client.PutAsync<CampusOut>("/campi", body);

        // Assert
        updatedCampus.Id.Should().Be(campus.Id);
        updatedCampus.Nome.Should().Be(body.Nome);
        updatedCampus.Cidade.Should().Be(body.Cidade);
    }

    [Test]
    public async Task Deve_retornar_apenas_os_campus_da_faculdade_do_usuario_logado()
    {
        // Arrange
        var client = _factory.CreateClient();
        var userNovaRoma = await client.NewAcademico("Nova Roma");
        var userUfpe = await client.NewAcademico("UFPE");

        await client.Login(userNovaRoma);
        await client.NewCampus("Agreste I", "Caruaru - PE");

        await client.Login(userUfpe);
        await client.NewCampus("Suassuna", "Recife - PE");

        // Act
        await client.Login(userNovaRoma);

        var campi = await client.GetAsync<List<CampusOut>>("/campi");
        campi.Should().HaveCount(1);

        // Assert
        campi[0].Id.Should().NotBeEmpty();
        campi[0].Nome.Should().Be("Agreste I");
        campi[0].Cidade.Should().Be("Caruaru - PE");
    }
}
